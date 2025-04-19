import matplotlib.pyplot as plt
import seaborn as sns
import pandas as pd


def clone_usage(df):
    clone_counts = df.groupby('level_name')['clone_usage_count'].sum()

    plt.figure(figsize=(8, 5))
    sns.barplot(x=clone_counts.index, y=clone_counts.values)
    plt.title('Clone Usage per Level')
    plt.xlabel('Level Name')
    plt.ylabel('Number of Clones Used')
    plt.xticks(rotation=45)
    plt.tight_layout()
    plt.savefig("Assets/Analytics/Graphs/CloneUsage/CloneUsage.png")


def clone_usage_boxplot(df):
    sns.boxplot(x='level_name', y='first_activation_time', data=df)
    plt.title('First Clone Activation Time per Level')
    plt.xlabel('Level')
    plt.ylabel('Time to First Clone Activation (seconds)')
    plt.xticks(rotation=45)
    medians = df.groupby('level_name')['first_activation_time'].median()
    for i, median in enumerate(medians):
        plt.text(i, median + 0.5, f"{median:.1f}s",
                 horizontalalignment='center', color='black')
    plt.tight_layout()
    plt.savefig("Assets/Analytics/Graphs/CloneUsage/FirstActivationTime.png")
