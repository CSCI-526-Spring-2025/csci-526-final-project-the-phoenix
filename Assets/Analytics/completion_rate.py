import matplotlib.pyplot as plt
import seaborn as sns
import pandas as pd
from firebase_admin import db


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


def plot_countplot(df):
    plt.figure(figsize=(8, 5))
    sns.countplot(x="level_name", data=df,
                  order=df["level_name"].value_counts().index)
    plt.title("Number of Completions per Level")
    plt.xlabel("Level Name")
    plt.ylabel("Number of Completions")
    plt.tight_layout()
    plt.savefig("Assets/Analytics/Graphs/LevelCompletion/LevelCompletion.png")


def plot_completion_rate(completed_players, total_players):
    completion_rate = (completed_players / total_players) * 100
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
