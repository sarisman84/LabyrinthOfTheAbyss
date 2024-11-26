# Version Control - Standard

When using version control, it is important to follow a set standard in order to minimize conflicts and organize the repository.

## Branch Structure
All development will be saved in the ``dev`` branch with the goal of having a complete set of features be saved on the ``build`` branch as versions:

![[Pasted image 20241126221428.png]]
## Using Branches as Tasks
Working only in ``dev`` will inevitably cause conflicts, which will slow down development if they occur often. For this reason, whenever you want to start working on a task, you [create a branch](https://www.youtube.com/watch?v=8-qRKyy-v7I&list=PLe6EXFvnTV78WqGmGSq8JPnafR3lAa55n&index=4&pp=iAQB) from the latest commit of ``dev``.

The branches name needs to describe the task that you are working on with the format being ``task-type/task-name``.

An example of this format for a gameplay task related to the player would be ``gameplay/player-controller``. If an artist wants to draw concept art of a playable character, the branch name would be ``concept/playable-character``.

If the task name becomes a sentence, such as ``gameplay/saving-system-for-the-player``, it is best to split the name into ``sub-task-types``. An example of this is ``gameplay/player/save-system``. 

You then continue to work on that branch for that specific task and once you are done with your task, you [pull request](https://www.youtube.com/watch?v=F5vHRyEtRAY&list=PLe6EXFvnTV78WqGmGSq8JPnafR3lAa55n&index=20&pp=iAQB) into ``dev``. The pull requests' name is the branch's name but capitalized and with spaces. It is optional to add reviewers, assignees or labels.

![[Pasted image 20241126224748.png]]

Once you have created the pull request, it will check if there are any [merge conflicts](https://www.youtube.com/watch?v=R1iWJNyRpQE&list=PLe6EXFvnTV78WqGmGSq8JPnafR3lAa55n&index=34&pp=iAQB) which will allow you to resolve them. After that is done or when you haven't found any merge conflicts, merge the branch to ``dev``. Should you wish to continue working on the task, you can leave your branch alive. Otherwise, it is best to delete the branch once you are done.

![[Pasted image 20241126225010.png]]