This game is a re-make of PaperIO/LandIO with a twist.


**PLAYER MOVEMENT**
Grid Movement is setup, the way it works is, it takes the input and scales it by the serialized speed field. And then it rounds it the nearest GridSquareSize and actually moves it again
This double approach to move -> Find adjustment to land on nearest square -> Move again, seems to be noted as more ideal for smoother movement, and it looks smooth enough.
Understand that the tiles generated are not actually tied to the movement in anyway, we are just spawning the chicken on a random tile's transform location and making sure it moves a unit square everytime.
**FIX NEEDED**
For some reason when i set the spawn point of the player, it doesnt accept a float, if i set it as (15.6,15.4) it goes to (16,15). What is this setting, i already turned snapping off (magnetic icon)
For this reason the vertical component of the movement doesnt land on squares since the x coordinate randomly landed on float and y was int. Could it be something in my trailwriter script position even without input.

**THE OUTSIDE BORDERS**
Theses are manually done and spaced out, each with a boxcollider2D attached to it. An ideal improvement would be to spawn these objects around the squares.
**FIX NEEDED**
When the player hits the wall, instead of coming to a dead stop, they actually glide slightly in the direction of the wall, they can also get stuck in a certain angle if they really bump into it well.
Either restrict movement more or change the collider.

**GRID GENERATION**
The grid is generated as 50x50 board. The issue is, right now it starts generating on the bottom left of the camera, this is because misunderstood what the camera should cover when i was first making this script.
Ideally this script can be redesigned such that instead of generating from the bottom left of wherever you have the camera, it generates on a point (1,1) and goes onwards

**Improvements**
Change the way the grid generates first.
Generate the borders around the grid after
Fix movement at border
