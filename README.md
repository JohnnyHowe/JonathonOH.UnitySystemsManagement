SystemsManagement is a Unity Singleton implementation. 

Before using this or any other singleton implementation please consider whether it is right for your project. Singletons have a lot of drawbacks. 

# How It Works/Why Use This Implementation
A prefab contains all your game systems. This prefab persists through scenes and is loaded in only when needed (lazy loading).

Using a prefab instead of having a separate initialization scene (like many other singleton implementations) means we don't have to run an init scene first - we can start anywhere we want.

We also don't have to open the init scene in editor to edit the singletons.

# How To Use
## 1. Install Package
In Unity 
1. Go to the Package Manager 
2. Click the little plus in the top left and "Add Package From git URL ..."
3. Paste `https://github.com/JohnnyHowe/com.jonathonoh.unity.systemsmanagement.git` and click add.

## 2. Create Systems prefab
This prefab must be in the root of a resources folder and called "Systems". This is where you'll put all of your system components.

I'd recommend putting them on child objects to keep it tidy. 
So the hierarchy might look like this:
```
Systems
 |- SystemA
 |- SystemB
 |- SystemC
 |- SystemD
```

## 3. Create a System
Just like any other Singleton

```
using JonathonOH.SystemsManagement;

public class ThingIWantToAccessEverywhere: GameSystem
{
    public void DoAThing()
    {
        ...
    }
}
```

## 4. Done
Now you can call it anywhere

```
ThingIWantToAccessEverywhere.Instance.DoAThing();