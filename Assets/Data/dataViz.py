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


def fetch_firebase_data():
    ref = db.reference("level_completion")
    data = ref.get()

    if not data:
        print("No data found in Firebase!")
        return None

    df = pd.DataFrame.from_dict(data, orient="index")
    return df


df = fetch_firebase_data()

if df is not None:
    print(len(df), "records fetched from Firebase.")

    df["timestamp"] = pd.to_datetime(df["timestamp"])
    df["completion_time"] = df["completion_time"].astype(int)
    df["level_name"] = df["level_name"].astype(str)

    plt.figure(figsize=(8, 5))
    sns.histplot(df["completion_time"], bins=10, kde=True)
    plt.title("Distribution of Level Completion Times")
    plt.xlabel("Completion Time (seconds)")
    plt.ylabel("Number of Players")
    plt.show()

    plt.figure(figsize=(8, 5))
    sns.countplot(y=df["level_name"],
                  order=df["level_name"].value_counts().index)
    plt.title("Number of Completions per Level")
    plt.xlabel("Number of Completions")
    plt.ylabel("Level Name")
    plt.show()
else:
    print("No data available to analyze.")
