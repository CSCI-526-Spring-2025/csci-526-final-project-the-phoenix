from matplotlib.colors import LinearSegmentedColormap
import firebase_admin
from firebase_admin import credentials, db
import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns


cred = credentials.Certificate(
    "/Users/namrathasairam/Phoenix/Assets/Analytics/doppledash-2a42c-firebase-adminsdk-fbsvc-951ff5d51d.json")
firebase_admin.initialize_app(cred, {
    "databaseURL": "https://doppledash-2a42c-default-rtdb.firebaseio.com/"
})


def fetch_firebase_data(db_name):
    ref = db.reference(db_name)
    data = ref.get()

    if not data:
        print("No data found in Firebase!")
        return None

    df = pd.DataFrame.from_dict(data, orient="index")
    return df


df_completion = fetch_firebase_data("level_completion")
df_deaths = fetch_firebase_data("player_deaths")
df_gravity_shift_counts = fetch_firebase_data("gravity_logs")

if df_completion.empty or df_deaths.empty:
    print("No data found in Firebase!")
else:
    # Count total players who played each level
    total_players = df_completion.groupby("level_name").size()

    # Count deaths per level
    deaths_per_level = df_deaths.groupby("level_name").size()
    deaths_per_level["Tutorial"] = 0
    print(deaths_per_level)

    # Compute Level Completion Rate
    completion_rate = (
        total_players - deaths_per_level).fillna(0) / total_players

    completion_rate = completion_rate * 100

    print("Level Completion Rate")
    print(completion_rate)

    plt.figure(figsize=(8, 5))
    sns.histplot(df_completion["completion_time"], bins=10, kde=True)
    plt.title("Distribution of Level Completion Times")
    plt.xlabel("Completion Time (seconds)")
    plt.ylabel("Number of Players")

    plt.figure(figsize=(8, 5))
    sns.countplot(x="level_name", data=df_completion,
                  order=df_completion["level_name"].value_counts().index)
    plt.title("Number of Completions per Level")
    plt.xlabel("Level Name")
    plt.ylabel("Number of Completions")

    plt.figure(figsize=(8, 5))
    sns.barplot(x=completion_rate.index,
                y=completion_rate.values, palette="viridis")
    plt.title("Level Completion Rate (%)")
    plt.xlabel("Level Name")
    plt.ylabel("Completion Rate (%)")
    plt.ylim(0, 100)
    plt.xticks(rotation=45)
    plt.show()

    # Heatmap for player and clone death

    custom_cmap = LinearSegmentedColormap.from_list(
        'custom_cmap', ['green', 'yellow', 'red']
    )

    # Filter player and clone deaths
    player_deaths = df_deaths[df_deaths['player_type'] == 'player']
    clone_deaths = df_deaths[df_deaths['player_type'] == 'clone']

    # Axis range
    x_range = range(0, 20, 2)
    y_range = range(0, 20, 2)

    # Player Heatmap
    plt.figure(figsize=(10, 6))
    player_plot = sns.kdeplot(
        x=player_deaths['death_x'],
        y=player_deaths['death_y'],
        fill=True,
        cmap=custom_cmap,
        bw_adjust=0.5,
        levels=100,
        thresh=0
    )
    plt.title('Heatmap of Player Deaths')
    plt.xlabel('X Position')
    plt.ylabel('Y Position')
    plt.xlim(0, 20)
    plt.ylim(0, 20)
    plt.xticks(x_range)
    plt.yticks(y_range)
    # Add colorbar properly
    mappable = player_plot.get_children()[0]
    plt.colorbar(mappable, label="Density of Deaths")
    plt.tight_layout()
    plt.show()

    # Clone Heatmap
    plt.figure(figsize=(10, 6))
    clone_plot = sns.kdeplot(
        x=clone_deaths['death_x'],
        y=clone_deaths['death_y'],
        fill=True,
        cmap=custom_cmap,
        bw_adjust=0.5,
        levels=100,
        thresh=0
    )
    plt.title('Heatmap of Clone Deaths')
    plt.xlabel('X Position')
    plt.ylabel('Y Position')
    plt.xlim(0, 20)
    plt.ylim(0, 20)
    plt.xticks(x_range)
    plt.yticks(y_range)
    # Add colorbar properly
    mappable = clone_plot.get_children()[0]
    plt.colorbar(mappable, label="Density of Deaths")
    plt.tight_layout()
    plt.show()

    # Heatmap for gravity shift

    player_data = []
    clone_data = []

    # Extract player data
    player_records = []
    for idx, row in df_gravity_shift_counts.iterrows():
        if isinstance(row['player'], list):
            for entry in row['player']:
                player_records.append({
                    'x': entry['x'],
                    'y': entry['y'],
                    'gravity_count': entry['gravity_count']
                })

    df_player = pd.DataFrame(player_records)

    # Extract clone data
    clone_records = []
    for idx, row in df_gravity_shift_counts.iterrows():
        if isinstance(row['clone'], list):
            for entry in row['clone']:
                clone_records.append({
                    'x': entry['x'],
                    'y': entry['y'],
                    'gravity_count': entry['gravity_count']
                })

    df_clone = pd.DataFrame(clone_records)

    df_player['x_bin'] = (df_player['x'] // 2) * 2
    df_player['y_bin'] = (df_player['y'] // 2) * 2

    df_clone['x_bin'] = (df_clone['x'] // 2) * 2
    df_clone['y_bin'] = (df_clone['y'] // 2) * 2

    # Create pivot tables for heatmaps
    pivot_player = df_player.pivot_table(
        index='y_bin', columns='x_bin', values='gravity_count', aggfunc='sum', fill_value=0
    )

    pivot_clone = df_clone.pivot_table(
        index='y_bin', columns='x_bin', values='gravity_count', aggfunc='sum', fill_value=0
    )

    # Plot Player Heatmap
    plt.figure(figsize=(8, 6))
    sns.heatmap(pivot_player, annot=True, fmt="d",
                cmap="YlOrRd", cbar=True, linewidths=0.5)
    plt.title('Gravity Shift Heatmap - Player')
    plt.xlabel('X Position (binned)')
    plt.ylabel('Y Position (binned)')
    plt.show()

    # Plot Clone Heatmap
    plt.figure(figsize=(8, 6))
    sns.heatmap(pivot_clone, annot=True, fmt="d",
                cmap="Blues", cbar=True, linewidths=0.5)
    plt.title('Gravity Shift Heatmap - Clone')
    plt.xlabel('X Position (binned)')
    plt.ylabel('Y Position (binned)')
    plt.show()
