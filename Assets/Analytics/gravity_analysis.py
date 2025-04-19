import matplotlib.pyplot as plt
import seaborn as sns
import pandas as pd


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


def process_gravity_shift(df, title, cmap, player_type):
    if df.empty:
        print(f"No gravity shift data for {title}. Skipping heatmap.")
        return

    levels = df['level_name'].unique()

    for level in levels:
        level_data = df[df['level_name'] == level]

        if level_data.empty:
            print(f"No data for {title} - {level}. Skipping heatmap.")
            continue

        level_data['x_bin'] = (level_data['x'] // 2) * 2
        level_data['y_bin'] = (level_data['y'] // 2) * 2

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
            f"Assets/Analytics/Graphs/GravityShift/{player_type}/{level} Heatmap.png")


def plot_gravity_shift_counts(shift_counts, title, color):
    plt.figure(figsize=(8, 5))
    sns.barplot(x=shift_counts.index, y=shift_counts.values, palette=color)
    plt.title(title)
    plt.xlabel('Level Name')
    plt.ylabel('Total Gravity Shifts')
    plt.xticks(rotation=45)
    plt.tight_layout()
    plt.savefig(f"Assets/Analytics/Graphs/GravityShift/{title}.png")
    plt.show()
