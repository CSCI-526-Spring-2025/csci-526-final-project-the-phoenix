import plotly.graph_objects as go
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


def clone_usage(df):
    plt.figure(figsize=(10, 6))

    clone_counts = df.groupby('level_name')[
        'clone_usage_count'].median().sort_values(ascending=True)

    ax = sns.barplot(x=clone_counts.index,
                     y=clone_counts.values, palette='viridis')

    plt.title('Median Clone Decay per Level', fontsize=14, weight='bold')
    plt.xlabel('Level Name', fontsize=12)
    plt.ylabel('Number of Clones Used', fontsize=12)
    plt.xticks(rotation=30, ha='right', fontsize=10)
    plt.yticks(fontsize=10)

    for i, val in enumerate(clone_counts.values):
        ax.text(i, val, f"{val}", ha='center',
                va='bottom', fontsize=10, weight='bold')

    plt.tight_layout()
    plt.savefig("Assets/Analytics/Graphs/CloneUsage/CloneUsage.png", dpi=300)
    plt.show()
    plt.close()


def clone_usage_boxplot(df):
    plt.figure(figsize=(10, 6))

    ax = sns.boxplot(
        x='level_name',
        y='first_activation_time',
        data=df,
        palette='Set2',
    )

    # Titles and labels
    plt.title('First Clone Activation Time per Level',
              fontsize=14, weight='bold')
    plt.xlabel('Level', fontsize=12)
    plt.ylabel('Time to First Clone Activation (seconds)', fontsize=12)
    plt.xticks(rotation=30, ha='right', fontsize=10)
    plt.yticks(fontsize=10)

    # Median labels on boxes
    medians = df.groupby('level_name')['first_activation_time'].median()
    for i, median in enumerate(medians):
        ax.text(i, median + 1, f"{median:.1f}s",
                ha='center', color='black', weight='bold', fontsize=9)

    # Final formatting
    plt.tight_layout()
    plt.savefig(
        "Assets/Analytics/Graphs/CloneUsage/FirstActivationTime.png", dpi=300)
    plt.show()
    plt.close()


df_clone_usage = fetch_firebase_data("clone_usage")
if not df_clone_usage.empty:
    clone_usage(df_clone_usage)
    clone_usage_boxplot(df_clone_usage)
