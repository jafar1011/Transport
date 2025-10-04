# Project

Hi, this is the flowchart of the tables at the moment. <br> I have added the tables to local sql server (My individual accounts by default is sqlserver not sqlite). <br>

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
    ---
## Change Log – 2 - 2025/10/4
- Users with Role "Driver" can now Add a full post with all details (name,phone,car,areas).
- Posts will be shown to all users  .
- Post author can delete his own posts.
- Search box that searches all posts by area.
- UI changes and css and icons for home view.
