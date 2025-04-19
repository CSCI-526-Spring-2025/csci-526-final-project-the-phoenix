import seaborn as sns
import pandas as pd
import matplotlib.pyplot as plt
import matplotlib.image as mpimg
from matplotlib.colors import LinearSegmentedColormap
from firebase_admin import credentials, db, initialize_app

cred_path = "Assets/Analytics/doppledash-2a42c-firebase-adminsdk-fbsvc-951ff5d51d.json"
initialize_app(credentials.Certificate(cred_path), {
    "databaseURL": "https://doppledash-2a42c-default-rtdb.firebaseio.com/"
})


def clean_dataframe(df):
    return df[df['level_name'] != 'q'] if 'level_name' in df.columns else df


def fetch_firebase_data(db_name):
    ref = db.reference(db_name)
    data = ref.get()
    if not data:
        print(f"No data found in Firebase for {db_name}!")
        return pd.DataFrame()
    df = pd.DataFrame.from_dict(data, orient="index")
    print(f"Fetched {len(df)} records from {db_name}")
    return clean_dataframe(df)


def plot_death_heatmap(df):
    level_bounds = {
        "Level1": [-13.5, 24, -2.5, 14.5],
        "Level2": [-13.5, 24, -2.5, 14.5],
        "Level3": [-17, 17.5, 0.5, 19.5],
        "Level4": [-20.5, 31.5, -3.5, 25],
    }

    for level in df['level_name'].unique():
        level_data = df[df['level_name'] == level]
        if level_data.empty:
            print(f"No data for {level}. Skipping.")
            continue

        bg_path = f"Assets/Analytics/LevelImages/{level}.png"
        try:
            bg_img = mpimg.imread(bg_path)
        except FileNotFoundError:
            print(f"No background image for level {level}. Skipping.")
            continue

        bounds = level_bounds.get(level, [-13.5, 24, -2.5, 14.5])
        x_min, x_max, y_min, y_max = bounds

        plt.figure(figsize=(12, 6))
        plt.imshow(bg_img, extent=[x_min, x_max,
                   y_min, y_max], aspect='auto', alpha=0.8)

        plt.hist2d(
            level_data['death_x'],
            level_data['death_y'],
            bins=[50, 50],
            range=[[x_min, x_max], [y_min, y_max]],
            cmap='magma',
            alpha=0.6
        )

        plt.colorbar(label='Death Density')
        plt.title(f'Death Heatmap - {level}')
        plt.xlabel('X Position')
        plt.ylabel('Y Position')
        plt.xlim(x_min, x_max)
        plt.ylim(y_min, y_max)
        plt.tight_layout()
        plt.savefig(
            f"Assets/Analytics/Graphs/PlayerDeaths/HeatMap/{level}_DeathHeatmap.png")
        plt.close()


def plot_deaths_by_obstacle(df):
    if 'level_name' not in df.columns or 'obstacle_type' not in df.columns:
        print("Missing 'level_name' or 'obstacle_type' in the dataframe.")
        return

    grouped = df.groupby(['level_name', 'obstacle_type']
                         ).size().reset_index(name='count')

    plt.figure(figsize=(12, 6))
    sns.barplot(data=grouped, x='level_name', y='count', hue='obstacle_type')

    plt.title('Deaths by Obstacle Type per Level')
    plt.xlabel('Level')
    plt.ylabel('Number of Deaths')
    plt.xticks(rotation=45)
    plt.legend(title='Obstacle Type')
    plt.tight_layout()
    plt.savefig(
        "Assets/Analytics/Graphs/PlayerDeaths/Obstacle/DeathsByObstacle.png")
    plt.close()


if __name__ == "__main__":
    df_deaths = fetch_firebase_data("player_deaths")
    if not df_deaths.empty:
        plot_death_heatmap(df_deaths)
        plot_deaths_by_obstacle(df_deaths)
    else:
        print("No data to plot.")
