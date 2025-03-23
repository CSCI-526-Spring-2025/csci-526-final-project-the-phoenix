import firebase_admin
from firebase_admin import credentials, db
import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns

cred = credentials.Certificate(
    "Assets/Data/doppledash-2a42c-firebase-adminsdk-fbsvc-7181bb7724.json")
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
