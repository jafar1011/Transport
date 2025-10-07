# Project

Hi, this is the flowchart of the tables at the moment. <br> I have added the tables to local sql server (My individual accounts by default is sqlserver not sqlite(this has been changed to sqlite)). <br>

📄 **[FlowChart V2](https://drive.google.com/file/d/1zkqRh20PhbxSPWsJ4BazyAK7lLSVKNWd/view?usp=drive_link)**

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
- Post author can delete his own posts.
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
- Added the driver's rating to his posts to be viewed by users when browsing
- users with role of "students" now have a page "My Driver" that contain their driver and his car information
