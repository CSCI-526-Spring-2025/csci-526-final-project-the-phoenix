import matplotlib.pyplot as plt
import seaborn as sns
import pandas as pd
from matplotlib.colors import LinearSegmentedColormap


def plot_heatmap(df, player_type, title_prefix):
    filtered_data = df[df['player_type'] == player_type]

    custom_cmap = LinearSegmentedColormap.from_list(
        'custom_cmap', ['green', 'yellow', 'red']
    )

    levels = filtered_data['level_name'].unique()

    for level in levels:
        level_data = filtered_data[filtered_data['level_name'] == level]

        if level_data.empty:
            print(f"No data for {title_prefix} - {level}. Skipping heatmap.")
            continue

        plt.figure(figsize=(10, 6))
        sns.kdeplot(
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
            f"Assets/Analytics/Graphs/PlayerDeaths/{player_type}/{level} DeathHeatmap.png")


def plot_pie_chart(df_completion, df_deaths):
    completion_counts = df_completion.groupby('level_name').size()
    death_counts = df_deaths.groupby('level_name').size()

    sns.set(style="whitegrid")

    plt.figure(figsize=(6, 6))
    plt.pie(completion_counts.values, labels=completion_counts.index,
            autopct='%1.1f%%', startangle=140)
    plt.title('Player Completions per Level')
    plt.axis('equal')
    plt.tight_layout()
    plt.show()

    plt.figure(figsize=(6, 6))
    plt.pie(death_counts.values, labels=death_counts.index,
            autopct='%1.1f%%', startangle=140)
    plt.title('Player Deaths per Level')
    plt.axis('equal')
    plt.tight_layout()
    plt.show()
