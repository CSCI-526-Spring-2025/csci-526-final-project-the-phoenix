import matplotlib.image as mpimg
import matplotlib.pyplot as plt
import seaborn as sns
import pandas as pd
from matplotlib.colors import LinearSegmentedColormap
import firebase_admin
from firebase_admin import credentials, db
import matplotlib.pyplot as plt
import matplotlib.image as mpimg


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


level_bounds = {
    "Level1": [-13.5, 24, -5.5, 17.5],
    "Level2": [-13.5, 24, -5.5, 17.5],
    "Level3": [-17, 17.5, -0.25, 19.5],
    "Level4": [-20.5, 31.5, -3.5, 25],
}


def process_gravity_shift_scatter_plot(df, title, cmap, player_type):
    if df.empty:
        print(f"No gravity shift data for {title}. Skipping scatter plots.")
        return

    levels = df['level_name'].unique()

    for level in levels:
        if "tutorial" in level.lower():
            print(f"Skipping tutorial level: {level}")
            continue

        level_data = df[df['level_name'] == level]

        if level_data.empty:
            print(f"No data for {title} - {level}. Skipping.")
            continue

        bounds = level_bounds.get(level, [-13.5, 24, -2.5, 14.5])
        x_min, x_max, y_min, y_max = bounds

        bg_path = f"Assets/Analytics/LevelImages/{level}.png"

        plt.figure(figsize=(12, 6))
        try:
            bg_img = mpimg.imread(bg_path)
            plt.imshow(bg_img, extent=[x_min, x_max,
                       y_min, y_max], aspect='auto', alpha=0.8)
        except FileNotFoundError:
            print(
                f"Background image not found for {level}. Proceeding without it.")

        scatter = plt.scatter(
            x=level_data['x'],
            y=level_data['y'],
            c=level_data['gravity_count'],
            cmap=cmap,
            s=60,
            edgecolors='black'
        )
        plt.colorbar(scatter, label="Gravity Usage Count")
        plt.title(f"{title} - {level}")
        plt.xlabel("X Position")
        plt.ylabel("Y Position")
        plt.xlim(x_min, x_max)
        plt.ylim(y_min, y_max)
        plt.grid(True)
        plt.tight_layout()
        plt.show()
        plt.savefig(
            f"Assets/Analytics/Graphs/GravityShift/{player_type}/{level}_ScatterWithBG.png")
        plt.close()


df_gravity_shift_counts = fetch_firebase_data("gravity_logs")
df_player = extract_gravity_data(df_gravity_shift_counts, 'player')
df_clone = extract_gravity_data(df_gravity_shift_counts, 'clone')

player_shifts_per_level = df_player.groupby(
    'level_name')['gravity_count'].sum()
clone_shifts_per_level = df_clone.groupby(
    'level_name')['gravity_count'].sum()


plot_gravity_shift_counts(player_shifts_per_level,
                          'Total Gravity Shifts - Player', 'YlOrRd')
plot_gravity_shift_counts(clone_shifts_per_level,
                          'Total Gravity Shifts - Clone', 'Blues')

process_gravity_shift_scatter_plot(
    df_player, 'Gravity Shift Scatterplot - Player', "YlOrRd", "player")
process_gravity_shift_scatter_plot(
    df_clone, 'Gravity Shift Scatterplot - Clone', "Blues", "clone")
