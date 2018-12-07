# SkillsProof
This is simple project with two tasks for customer to prove our Unity3d skills. The implementation took around 3 hours.

### 1. Diamond miner

In this task we have global variable for diamonds count in PlayerPrefs that can be accessed from each miner, but all miners are independent from each other. 

The first part of this task is included the calculation of how much diamonds one miner produces per frame, than we increment global value with this delta value. 

The second part was to calculate how much diamonds was produced when app was closed. To do this we calculated time that user was offline and calculated the amount of diamonds that he could produce for this time, then we add this amount to global value.
Initially we have one original miner on the scene. Only this miner can print values in console (to prevent spam if there's a lot of miners). 

You can drag new miner from prefabs or just copy original one.

### 2. Custom scrollview

The idea was to exand base ScrollRect class to override OnBeginDrag and OnEndDrag events. In OnEndDrag we are seaching the closest button to center, and then calculate distance from it to center and smoothly scroll our content to wanted position. 

We user Lerp function inside coroutine to make scrolling smooth. When user clicks on button in scroll it instantiates a copy of clicked button to the end of content. 

Pivot of content was set to the left side to prevent buttons "jumping" when user instantiates even amount of buttons.
