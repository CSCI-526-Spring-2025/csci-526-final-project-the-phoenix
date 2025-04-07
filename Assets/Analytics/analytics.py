from matplotlib.colors import LinearSegmentedColormap
import firebase_admin
from firebase_admin import credentials, db
import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns

# Initialize Firebase
cred_path = "Assets/Analytics/doppledash-2a42c-firebase-adminsdk-fbsvc-951ff5d51d.json"
cred = credentials.Certificate(cred_path)
firebase_admin.initialize_app(cred, {
    "databaseURL": "https://doppledash-2a42c-default-rtdb.firebaseio.com/"
})


def fetch_firebase_data(db_name):
    ref = db.reference(db_name)
    data = ref.get()
    if not data:
        print(f"No data found in Firebase for {db_name}!")
        return pd.DataFrame()
    df = pd.DataFrame.from_dict(data, orient="index")
    print(f"Fetched {len(df)} records from {db_name}")
    return df


def plot_histogram(df):
    plt.figure(figsize=(8, 5))
    sns.histplot(df["completion_time"], bins=10, kde=True)
    plt.title("Distribution of Level Completion Times")
    plt.xlabel("Completion Time (seconds)")
    plt.ylabel("Number of Players")
    plt.tight_layout()
    plt.show()


def plot_countplot(df):
    plt.figure(figsize=(8, 5))
    sns.countplot(x="level_name", data=df,
                  order=df["level_name"].value_counts().index)
    plt.title("Number of Completions per Level")
    plt.xlabel("Level Name")
    plt.ylabel("Number of Completions")
    plt.tight_layout()
    plt.show()


def plot_completion_rate(total_players, deaths_per_level):
    completion_rate = (
        total_players - deaths_per_level).fillna(0) / total_players * 100
    print("Level Completion Rate (%)")
    print(completion_rate)

    plt.figure(figsize=(8, 5))
    sns.barplot(x=completion_rate.index,
                y=completion_rate.values, palette="viridis")
    plt.title("Level Completion Rate (%)")
    plt.xlabel("Level Name")
    plt.ylabel("Completion Rate (%)")
    plt.ylim(0, 100)
    plt.xticks(rotation=45)
    plt.tight_layout()
    plt.show()


def plot_heatmap(df, player_type, title):
    deaths = df[df['player_type'] == player_type]

    if deaths.empty:
        print(f"No death data for {player_type}. Skipping heatmap.")
        return

    custom_cmap = LinearSegmentedColormap.from_list(
        'custom_cmap', ['green', 'yellow', 'red'])
    plt.figure(figsize=(10, 6))
    plot = sns.kdeplot(
        x=deaths['death_x'],
        y=deaths['death_y'],
        fill=True,
        cmap=custom_cmap,
        bw_adjust=0.5,
        levels=100,
        thresh=0
    )
    plt.title(title)
    plt.xlabel('X Position')
    plt.ylabel('Y Position')
    plt.xlim(0, 20)
    plt.ylim(0, 20)
    plt.xticks(range(0, 21, 2))
    plt.yticks(range(0, 21, 2))
    mappable = plot.get_children()[0]
    plt.colorbar(mappable, label="Density of Deaths")
    plt.tight_layout()
    plt.show()


def process_gravity_shift(df, title, cmap):
    if df.empty:
        print(f"No gravity shift data for {title}. Skipping heatmap.")
        return

    df['x_bin'] = (df['x'] // 2) * 2
    df['y_bin'] = (df['y'] // 2) * 2

    pivot = df.pivot_table(
        index='y_bin',
        columns='x_bin',
        values='gravity_count',
        aggfunc='sum',
        fill_value=0
    )

    plt.figure(figsize=(8, 6))
    sns.heatmap(pivot, annot=True, fmt="d",
                cmap=cmap, cbar=True, linewidths=0.5)
    plt.title(title)
    plt.xlabel('X Position (binned)')
    plt.ylabel('Y Position (binned)')
    plt.tight_layout()
    plt.show()


def extract_gravity_data(df, entity_type):
    records = []
    for _, row in df.iterrows():
        if isinstance(row.get(entity_type), list):
            for entry in row[entity_type]:
                records.append({
                    'x': entry['x'],
                    'y': entry['y'],
                    'gravity_count': entry['gravity_count']
                })
    return pd.DataFrame(records)


def clone_usage(df):
    clone_counts = df.groupby('level_name')['clone'].sum()
    clone_counts = clone_counts[clone_counts > 0]
    print("Levels with Clone Usage:")
    print(clone_counts)
    return clone_counts


# Fetch data
df_completion = fetch_firebase_data("level_completion")
df_deaths = fetch_firebase_data("player_deaths")
df_gravity_shift_counts = fetch_firebase_data("gravity_logs")
df_clone_usage = fetch_firebase_data("clone_usage")

# Check data
if df_completion.empty or df_deaths.empty:
    print("Required data missing. Exiting.")
else:
    # Analysis
    total_players = df_completion.groupby("level_name").size()
    deaths_per_level = df_deaths.groupby("level_name").size()
    deaths_per_level["Tutorial"] = deaths_per_level.get("Tutorial", 0)

    plot_histogram(df_completion)
    plot_countplot(df_completion)
    plot_completion_rate(total_players, deaths_per_level)

    # Heatmaps
    plot_heatmap(df_deaths, 'player', 'Heatmap of Player Deaths')
    plot_heatmap(df_deaths, 'clone', 'Heatmap of Clone Deaths')

    # Gravity shift analysis
    df_player = extract_gravity_data(df_gravity_shift_counts, 'player')
    df_clone = extract_gravity_data(df_gravity_shift_counts, 'clone')

    process_gravity_shift(
        df_player, 'Gravity Shift Heatmap - Player', "YlOrRd")
    process_gravity_shift(df_clone, 'Gravity Shift Heatmap - Clone', "Blues")
