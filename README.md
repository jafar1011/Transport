# Project (Finished)

Hi, this is the flowchart of the tables at the moment. <br> I have added the tables to local sql server (My individual accounts by default is sqlserver not sqlite(this has been changed to sqlite)). <br>


## WorkFlow Walkthrough
- Driver & Student:
- Driver registers -> Verification request sent to admin -> Verified ->  Driver adds post.
- Student registers -> Browses posts -> Contacts Driver -> Driver sends invite to student by his phone number.
- Student accepts invite -> Student is now linked to the drivers as passenger.
- Parent & Student:
- Parent registers -> Parent sends invite to student by his phone number.
- Student accepts invite -> Student is now linked to the parent.


<br>

📄 **[FlowChart V3](https://drive.google.com/file/d/1hsIjJOVzErfqHllzKjKEcuNCurKqNMnz/view?usp=drive_link)**

<img width="278" height="424" alt="Database Flowchart" src="https://github.com/user-attachments/assets/52dee91f-0c99-4263-8b0f-2c0f68162144" />

---


## Change Log – 1 - 2025/10/3

- Replaced the **User** table with a **Student** table.  
- Using **IdentityUser** for all users.  
- Users can select a **role**: `Driver`, `Student`, or `Parent`.  
- UI dynamically adapts based on the selected role:  
  - **Driver:** Add/edit car information.  
  - **Student:** Manage academic information.  
  - **Parent:** Manage linked student information.
 
    <br>
    
## Change Log – 2 - 2025/10/4
- Users with Role "Driver" can now add a full post with all details (name,phone,car,areas).
- Posts will be shown to all users.
- Only post author can delete his own posts.
- Search box that searches all posts by area.
- UI changes and css and icons for home view.
- Swapped to sqlite instead of sqlserver so the database will be included in the project.
<br>

## Change Log – 3 - 2025/10/5
- Added passengers controller with its view (for drivers only).
- A driver can add (link) a passenger to its car using phone number.
- The driver will be able to see all passengers information in that page aswell.
- The student will be assigned to that specific car.

<br>
<img width="574" height="542" alt="image" src="https://github.com/user-attachments/assets/59053608-b8e8-4b93-8bf1-c06e33e03601" />

## Change Log – 4 - 2025/10/7
- Added ratings to drivers posts to be viewed by users when browsing
- users with role of "students" now have a page "My Driver" that contain their driver and his car information
<br>

## Change Log – 5 - 2025/10/10
- Implemented the rating system fully , now Students can rate their own driver and that driver rating's average will be shown.
- Drivers can "alert on arrive" to notify students of arrival.
- Invites System implemented where drivers/parents won't be able assign
the respective student until that student accept their invite (security measure).
- parents can now link a student to themself and be able to see their info with a "Track" button (not implement with google maps/gps).
- Created Ratings Table / Invites Table to hold that respective information.
- Renames / enhancements / some fixes.

## Change Log – 6 - 2025/10/11
- Drivers can not see their own ratings now (security measure).
- CSS / Bootstrap enhancements.


## Change Log – 7 - 2025/10/16
- Admin control page added.
- A master admin email (em: gaferadmin@gmail.com pw: Abcd1234!).
- An admin can delete any post.
- Can add more admins through the admin dashboard.
- Can disable driver accounts through admin dashboard.
- A Verification request will be sent to admins when a driver registers.
- Drivers now can not add posts until an admin verify their account through admin dashboard (security measure).


<<<<<<< HEAD
## Change Log – 8 And Final - 2025/10/17
- CSS enhancements.
- Admin can now manage all users data and change them throguh admin dashboard.
- Fixed user not being able to change his info.
- Database cleanup and enhancements


=======
## Change Log – 8 - 2025/10/17
- CSS enhancements.
>>>>>>> 3e1025d1edb5546894b9b98f674067d790add104
