from matplotlib.colors import LinearSegmentedColormap
import firebase_admin
from firebase_admin import credentials, db
import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns

# Initialize Firebase
# cred_path = "Assets/Analytics/doppledash-2a42c-firebase-adminsdk-fbsvc-951ff5d51d.json"
cred_path = "/Users/namrathasairam/Phoenix/Assets/Analytics/doppledash-2a42c-firebase-adminsdk-fbsvc-951ff5d51d.json"

cred = credentials.Certificate(cred_path)
firebase_admin.initialize_app(cred, {
    "databaseURL": "https://doppledash-2a42c-default-rtdb.firebaseio.com/"
})


def clean_dataframe(df):
    if 'level_name' in df.columns:
        return df[df['level_name'] != 'q']
    return df


def fetch_firebase_data(db_name):
    ref = db.reference(db_name)
    data = ref.get()
    if not data:
        print(f"No data found in Firebase for {db_name}!")
        return pd.DataFrame()
    df = pd.DataFrame.from_dict(data, orient="index")

    print(f"Fetched {len(df)} records from {db_name}")
    return clean_dataframe(df)


def plot_histogram(df):
    plt.figure(figsize=(8, 5))
    sns.boxplot(x='level_name', y='completion_time', data=df)
    plt.title('Distribution of Level Completion Time')
    plt.xlabel('Level')
    plt.ylabel('Completion Time (seconds)')
    plt.xticks(rotation=45)
    plt.tight_layout()
    plt.savefig(
        "Assets/Analytics/Graphs/LevelCompletion/LevelCompletionTime.png")
    # plt.show()


def plot_countplot(df):
    plt.figure(figsize=(8, 5))
    sns.countplot(x="level_name", data=df,
                  order=df["level_name"].value_counts().index)
    plt.title("Number of Completions per Level")
    plt.xlabel("Level Name")
    plt.ylabel("Number of Completions")
    plt.tight_layout()
    plt.savefig(
        "Assets/Analytics/Graphs/LevelCompletion/LevelCompletion.png")
    # plt.show()


def plot_completion_rate(completed_players, total_players):
    completion_rate = (
        completed_players / total_players) * 100

    plt.figure(figsize=(8, 5))
    sns.barplot(x=completion_rate.index,
                y=completion_rate.values, palette="viridis")
    plt.title("Level Completion Rate (%)")
    plt.xlabel("Level Name")
    plt.ylabel("Completion Rate (%)")
    plt.ylim(0, 100)
    plt.xticks(rotation=45)
    plt.tight_layout()
    plt.savefig(
        "Assets/Analytics/Graphs/LevelCompletion/LevelCompletionRate.png")
    # plt.show()


def plot_heatmap(df, player_type, title_prefix):
    # Filter by player type first
    filtered_data = df[df['player_type'] == player_type]

    # Custom color map
    custom_cmap = LinearSegmentedColormap.from_list(
        'custom_cmap', ['green', 'yellow', 'red']
    )

    # Group by level
    levels = filtered_data['level_name'].unique()

    for level in levels:
        level_data = filtered_data[filtered_data['level_name'] == level]

        if level_data.empty:
            print(f"No data for {title_prefix} - {level}. Skipping heatmap.")
            continue

        plt.figure(figsize=(10, 6))
        plot = sns.kdeplot(
            x=level_data['death_x'],
            y=level_data['death_y'],
            fill=True,
            cmap=custom_cmap,
            bw_adjust=0.5,
            levels=100,
            thresh=0
        )

        plt.title(f'{title_prefix} Heatmap - {level}')
        plt.xlabel('X Position')
        plt.ylabel('Y Position')
        plt.xlim(0, 20)
        plt.ylim(0, 20)
        plt.xticks(range(0, 21, 2))
        plt.yticks(range(0, 21, 2))

        plt.tight_layout()
        plt.savefig(
            "Assets/Analytics/Graphs/PlayerDeaths/{player_type}/{level} DeathHeatmap.png".format(
                player_type=player_type, level=level
            )
        )
        # plt.show()


def process_gravity_shift(df, title, cmap, player_type):
    if df.empty:
        print(f"No gravity shift data for {title}. Skipping heatmap.")
        return

    print("DF is", df)
    levels = df['level_name'].unique()

    for level in levels:
        level_data = df[df['level_name'] == level]

        if level_data.empty:
            print(f"No data for {title} - {level}. Skipping heatmap.")
            continue

        # Bin the positions
        level_data['x_bin'] = (level_data['x'] // 2) * 2
        level_data['y_bin'] = (level_data['y'] // 2) * 2

        # Create pivot table
        pivot = level_data.pivot_table(
            index='y_bin',
            columns='x_bin',
            values='gravity_count',
            aggfunc='sum',
            fill_value=0
        )

        plt.figure(figsize=(8, 6))
        sns.heatmap(pivot, annot=True, fmt="d",
                    cmap=cmap, cbar=True, linewidths=0.5)
        plt.title(f'{title} - {level}')
        plt.xlabel('X Position (binned)')
        plt.ylabel('Y Position (binned)')
        plt.tight_layout()
        plt.savefig(
            "Assets/Analytics/Graphs/GravityShift/{player_type}/{level} Heatmap.png".format(
                player_type=player_type, level=level
            )
        )
        # plt.show()


def plot_gravity_shift_counts(shift_counts, title, color):
    plt.figure(figsize=(8, 5))
    sns.barplot(x=shift_counts.index, y=shift_counts.values, palette=color)
    plt.title(title)
    plt.xlabel('Level Name')
    plt.ylabel('Total Gravity Shifts')
    plt.xticks(rotation=45)
    plt.tight_layout()
    plt.savefig(
        "Assets/Analytics/Graphs/GravityShift/{title}.png".format(
            title=title
        )
    )
    plt.show()


def plot_pie_chart(df_completion, df_deaths):
    # Group data
    completion_counts = df_completion.groupby('level_name').size()
    death_counts = df_deaths.groupby('level_name').size()

    # Set style
    sns.set(style="whitegrid")

    plt.figure(figsize=(6, 6))
    plt.pie(completion_counts.values, labels=completion_counts.index,
            autopct='%1.1f%%', startangle=140)
    plt.title('Player Completions per Level')
    # Equal aspect ratio ensures that pie is drawn as a circle.
    plt.axis('equal')
    plt.tight_layout()
    plt.show()

    # Player Deaths Pie Chart
    plt.figure(figsize=(6, 6))
    plt.pie(death_counts.values, labels=death_counts.index,
            autopct='%1.1f%%', startangle=140)
    plt.title('Player Deaths per Level')
    plt.axis('equal')
    plt.tight_layout()
    plt.show()


def extract_gravity_data(df, entity_type):
    records = []
    for _, row in df.iterrows():
        level = row.get('level_name')
        if isinstance(row.get(entity_type), list):
            for entry in row[entity_type]:
                records.append({
                    'x': entry['x'],
                    'y': entry['y'],
                    'gravity_count': entry['gravity_count'],
                    'level_name': level
                })
    return pd.DataFrame(records)


def clone_usage(df):
    clone_counts = df.groupby('level_name')['clone_usage_count'].sum()

    plt.figure(figsize=(8, 5))
    sns.barplot(x=clone_counts.index, y=clone_counts.values)
    plt.title('Clone Usage per Level')
    plt.xlabel('Level Name')
    plt.ylabel('Number of Clones Used')
    plt.xticks(rotation=45)
    plt.tight_layout()
    plt.savefig(
        "Assets/Analytics/Graphs/CloneUsage/CloneUsage.png")
    # plt.show()


def clone_usage_boxplot(df):
    sns.boxplot(x='level_name', y='first_activation_time', data=df)
    plt.title('First Clone Activation Time per Level')
    plt.xlabel('Level')
    plt.ylabel('Time to First Clone Activation (seconds)')
    plt.xticks(rotation=45)
    medians = df.groupby('level_name')['first_activation_time'].median()
    for i, median in enumerate(medians):
        plt.text(i, median + 0.5, f"{median:.1f}s",
                 horizontalalignment='center', color='black')
    plt.tight_layout()
    plt.savefig(
        "Assets/Analytics/Graphs/CloneUsage/FirstActivationTime.png")
    # plt.show()


# Fetch data
df_completion = fetch_firebase_data("level_completion")
df_deaths = fetch_firebase_data("player_deaths")
df_gravity_shift_counts = fetch_firebase_data("gravity_logs")
df_clone_usage = fetch_firebase_data("clone_usage")
df_players = fetch_firebase_data("players")

# Check data
if df_completion.empty or df_deaths.empty:
    print("Required data missing. Exiting.")
else:
    # Analysis
    completed_players = df_completion.groupby("level_name").size()
    total_players = df_players.groupby("level_name").size()

    plot_histogram(df_completion)
    plot_countplot(df_completion)
    plot_completion_rate(completed_players, total_players)

    # Heatmaps
    plot_heatmap(df_deaths, "player", "Player")
    plot_heatmap(df_deaths, "clone", "Clone")

    # Clone usage analysis
    clone_usage(df_clone_usage)
    clone_activation_times = df_clone_usage.groupby(
        'level_name')['first_activation_time'].median()

    clone_usage_boxplot(df_clone_usage)

    # Gravity shift analysis
    df_player = extract_gravity_data(df_gravity_shift_counts, 'player')
    df_clone = extract_gravity_data(df_gravity_shift_counts, 'clone')

    process_gravity_shift(
        df_player, 'Gravity Shift Heatmap - Player', "YlOrRd", "player")
    process_gravity_shift(
        df_clone, 'Gravity Shift Heatmap - Clone', "Blues", "clone")

    player_shifts_per_level = df_player.groupby(
        'level_name')['gravity_count'].sum()
    clone_shifts_per_level = df_clone.groupby(
        'level_name')['gravity_count'].sum()

    player_avg_shifts = df_player.groupby('level_name')['gravity_count'].mean()
    clone_avg_shifts = df_clone.groupby('level_name')['gravity_count'].mean()

    plot_gravity_shift_counts(player_shifts_per_level,
                              'Total Gravity Shifts - Player', 'YlOrRd')
    plot_gravity_shift_counts(clone_shifts_per_level,
                              'Total Gravity Shifts - Clone', 'Blues')

    plot_gravity_shift_counts(
        player_avg_shifts, 'Average Gravity Shifts - Player', 'YlOrRd')
    plot_gravity_shift_counts(
        clone_avg_shifts, 'Average Gravity Shifts - Clone', 'Blues')

    # Miletone analysis (piecharts)
    plot_pie_chart(df_completion, df_deaths)
