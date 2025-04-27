import matplotlib.pyplot as plt
import pandas as pd
from firebase_admin import credentials, db, initialize_app

# Firebase setup
cred_path = "Assets/Analytics/doppledash-2a42c-firebase-adminsdk-fbsvc-951ff5d51d.json"
initialize_app(credentials.Certificate(cred_path), {
    "databaseURL": "https://doppledash-2a42c-default-rtdb.firebaseio.com/"
})


def clean_dataframe(df):
    discard_levels = ['q', 'Tutorial', 'Camera Trial', 'Test', 'Level5']
    return df[df['level_name'].isin(discard_levels) == False] if 'level_name' in df.columns else df


def fetch_firebase_data(db_name):
    ref = db.reference(db_name)
    data = ref.get()
    if not data:
        print(f"No data found in Firebase for {db_name}!")
        return pd.DataFrame()
    df = pd.DataFrame.from_dict(data, orient="index")
    print(f"Fetched {len(df)} records from {db_name}")
    return clean_dataframe(df)


# Fetch data
df_completion = fetch_firebase_data("level_completion")
df_deaths = fetch_firebase_data("player_deaths")

# Group and count
completion_counts = df_completion['level_name'].value_counts()
death_counts = df_deaths['level_name'].value_counts()

# Calculate total
total_completions = completion_counts.sum()
total_deaths = death_counts.sum()

# Percentages
completion_percentages = (
    completion_counts / total_completions * 100).sort_index()
death_percentages = (death_counts / total_deaths * 100).sort_index()

# Define custom colors
completion_colors = ['#4CAF50', '#2196F3', '#FFC107',
                     '#FF5722', '#9C27B0']  # Green, Blue, Amber, Orange, Purple
death_colors = ['#F44336', '#FF9800', '#9C27B0', '#3F51B5',
                '#00BCD4']       # Red, Orange, Purple, Blue, Cyan

# Plot
fig, axes = plt.subplots(1, 2, figsize=(12, 6))

# Completions Pie
axes[0].pie(completion_percentages, labels=completion_percentages.index,
            autopct='%1.1f%%', startangle=140, colors=completion_colors)
axes[0].set_title('Player Completions per Level')

# Deaths Pie
axes[1].pie(death_percentages, labels=death_percentages.index,
            autopct='%1.1f%%', startangle=140, colors=death_colors)
axes[1].set_title('Player Deaths per Level')

plt.tight_layout()
plt.show()
