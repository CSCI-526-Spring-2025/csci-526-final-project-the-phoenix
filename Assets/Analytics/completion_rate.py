import matplotlib.pyplot as plt
import seaborn as sns
import pandas as pd
from firebase_admin import db
from firebase_admin import credentials, db
import firebase_admin
import numpy as np


cred_path = "Assets/Analytics/doppledash-2a42c-firebase-adminsdk-fbsvc-951ff5d51d.json"

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


def plot_boxplot(df):
    plt.figure(figsize=(8, 5))
    sns.boxplot(x='level_name', y='completion_time', data=df,
                palette='Set2',
                )
    plt.title('Distribution of Level Completion Time')
    plt.xlabel('Level')
    plt.ylabel('Completion Time (seconds)')
    plt.xticks(rotation=45)
    plt.tight_layout()
    plt.savefig(
        "Assets/Analytics/Graphs/LevelCompletion/LevelCompletionTime.png")
    plt.show()
    plt.close()


def plot_completion_rate(completed_players, total_players):
    completion_rate = (completed_players / total_players) * 100
    plt.figure(figsize=(8, 5))
    sns.barplot(x=completion_rate.index,
                y=completion_rate.values,
                hue=completion_rate.index,
                palette="viridis",
                legend=False)
    plt.title("Level Completion Rate (%)")
    plt.xlabel("Level Name")
    plt.ylabel("Completion Rate (%)")
    plt.ylim(0, 30)
    plt.xticks(rotation=45)
    plt.tight_layout()
    plt.savefig(
        "Assets/Analytics/Graphs/LevelCompletion/LevelCompletionRate.png")
    plt.show()
    plt.close()


def plot_level_completion_time(df):
    plt.figure(figsize=(8, 5))
    avg_time = df.groupby('level_name')['completion_time'].mean().sort_values()

    sns.barplot(x=avg_time.index,
                y=avg_time.values,
                palette="coolwarm")

    plt.title('Average Level Completion Time')
    plt.xlabel('Level Name')
    plt.ylabel('Average Completion Time (seconds)')
    plt.xticks(rotation=45)
    plt.tight_layout()
    plt.savefig(
        "Assets/Analytics/Graphs/LevelCompletion/AverageCompletionTime.png")
    plt.show()
    plt.close()


def plot_players_who_left_per_level(df_players, df_completion, df_deaths):
    # Group by level
    players_per_level = df_players.groupby('level_name')['player_id'].nunique()
    completion_per_level = df_completion.groupby(
        'level_name')['player_id'].nunique()
    deaths_per_level = df_deaths.groupby('level_name')['player_id'].nunique()
    # Players who left without dying or completing
    players_who_left = players_per_level - completion_per_level - deaths_per_level

    plt.figure(figsize=(10, 6))
    players_who_left.plot(kind='bar', color='#FF7F50')
    plt.title('Players Who Left Without Dying or Completing (Per Level)')
    plt.xlabel('Level')
    plt.ylabel('Number of Players')
    plt.xticks(rotation=45, ha='right')
    plt.grid(axis='y', linestyle='--', alpha=0.7)
    plt.tight_layout()
    plt.show()

    return players_who_left


df_players = fetch_firebase_data("players")
df_completion = fetch_firebase_data("level_completion")
df_deaths = fetch_firebase_data("player_deaths")

discarded_levels = ['q', 'Camera Trial', 'Tutorial', 'Test', 'Level4']
df_players = df_players[df_players['level_name'].isin(
    discarded_levels) == False]
df_completion = df_completion[df_completion['level_name'].isin(
    discarded_levels) == False]
df_deaths = df_deaths[df_deaths['level_name'].isin(
    discarded_levels) == False]
completed_players = df_completion.groupby("level_name").size()
total_players = df_players.groupby("level_name").size()

plot_boxplot(df_completion)
plot_completion_rate(completed_players, total_players)
plot_level_completion_time(df_completion)
plot_players_who_left_per_level(
    df_players, df_completion, df_deaths)
