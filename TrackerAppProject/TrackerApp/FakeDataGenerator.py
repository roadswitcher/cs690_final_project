import json
import random
from datetime import datetime, timedelta

# Define the list of moods
moods = ["Happy", "Sad", "Angry", "Wistful", "Indifferent", "Anxious", "Excited", "Frustrated", "Content"]

# Define the triggers
triggers = [
    "Just got an A on my CS690 assignment",
    "Had a great conversation with a friend",
    "Struggling with a difficult homework problem",
    "Looking forward to the weekend",
    "Feeling overwhelmed with schoolwork",
    "Just got a good cup of coffee",
    "Had a frustrating conversation with a professor",
    "Excited for my afternoon class",
    "Feeling anxious about an upcoming exam",
    "Just finished a long project",
    "Had a great time at the gym",
    "Feeling stressed about deadlines",
    "Just got a good grade on a quiz",
    "Had a nice walk outside",
    "Feeling tired and need a nap"
]

# Define the user's schedule
schedule = {
    "Tuesday": {"afternoon_class": "CS690"},
    "Thursday": {"afternoon_class": "CS690"},
    "Friday": {"morning_lab": "Not enjoying lab"}
}

# Function to generate a random mood update
def generate_mood_update(date):
    mood = random.choice(moods)
    trigger = random.choice(triggers) if random.random() < 0.5 else None
    hour = random.randint(7, 23)  # Random hour between 7am and 11pm
    minute = random.randint(0, 59)  # Random minute
    second = random.randint(0, 59)  # Random second
    microsecond = random.randint(0, 999999)  # Random microsecond
    timestamp = date.replace(hour=hour, minute=minute, second=second, microsecond=microsecond).isoformat() + 'Z'
    return {
        "Mood": mood,
        "Trigger": trigger,
        "Timestamp": timestamp
    }

# Function to generate mood updates for a day
def generate_day(date):
    day_of_week = date.strftime("%A")
    updates = []
    for _ in range(random.randint(3, 15)):  # Random number of updates between 3 and 15
        update = generate_mood_update(date)
        if day_of_week in schedule:
            if "afternoon_class" in schedule[day_of_week] and update["Timestamp"].startswith(date.strftime("%Y-%m-%dT14")):
                update["Trigger"] = f"Just got out of my {schedule[day_of_week]['afternoon_class']} class"
            elif "morning_lab" in schedule[day_of_week] and update["Timestamp"].startswith(date.strftime("%Y-%m-%dT09")):
                update["Trigger"] = schedule[day_of_week]["morning_lab"]
        updates.append(update)
    return updates

# Generate 45 days worth of mood updates
start_date = datetime.now() - timedelta(days=45)
mood_data = {
    "UserCredentials": {
        "Username": "Zoe"
    },
    "MoodRecords": []
}
for i in range(45):
    date = start_date + timedelta(days=i)
    mood_data["MoodRecords"].extend(generate_day(date))

# Save the mood updates to a JSON file
with open("mood_data.json", "w") as f:
    json.dump(mood_data, f, indent=4)