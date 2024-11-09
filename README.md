# MRBS
Project 2 Booking System

Actors
- Admin
- User (Will Book the Room)

Rules
USER MANAGEMENT
- Admin will add new users with Dets (Name, Email, Role, Password). Should have Super Admin that will manage the Admins
-----------------------------------------------------------------------------------
ROLES
- Admin (Can manage all users, create and manage room bookings, and modify system settings). The admin can directly assign roles to users, ensuring that each user has the correct permissions. For instance, a staff member could be given access to manage bookings, while a regular user might only be able to book rooms for themselves.
- Staff (Can create and manage room bookings for clients).
----------------------------------------------------------------------------------
- Users can edit their profiles (Details), including the role changes and password resets.
- Admins can delete user accounts from the system. Super Admins can delete the Admin Accounts
- View Users, display all the users with their Dets and roles

USERS
- SUPER ADMIN, ADMIN and STAFF(User)

USER AUTH
- Login and Logout, validate user credentials and creates a session or JWT token for Authentication. For logout, clears user session or token


ROOM MANAGEMENT
- Add Room, only ADMIN can add new Meeting Rooms with its respective details (Name, Location, Capacity, and available Facilities)
- Edit Room, only ADMIN can edit room informations in the database
- Delete Room, only ADMIN can remove meting rooms from the database
- View Rooms, STAFF (User) all rooms will be displayed with their Details (Display can be found in the dashboard in a list view)


BOOKING MANAGEMENT
- Create Booking (Staff/User), Users can book a meeting room by selecting room, date, time and duration
- Edit Booking (Staff/User), Enables users to update their existing booking (Change Date, Update Duration. Should not be in conflict with other bookings)
-Cancel Bookings (Staff/User), allows user to cancel their bookings (with terms and other prior consideration)
- View Bookings(Staff/User), Display a list of all bookings, with option FILTERS (by room, date and User). Every User in the application has a different display of its own bookings, admin will be viewing all the bookings


RECURRING BOOKINGS
- Create Recurring Booking (Staff/User), users can book a specific room that repeats at specified intervals (Meaning User can book the meeting room every Tuesday 9am - 12pm). Either Daily or Weekly or Monthly)
- Edit Recurring Booking (Staff/User), users to update their recurring dets (time, date, durations, considering other bookings, should not be in conflict with others. If recurring bookings updated, if its in conflict with other bookings after the update the system will respond a unable to book meeting room (POP UP Error "Unable to Book Meeting Room on Tuesday March 6, 2025).
- Cancel Recurring Booking (Staff/User), all user booking cancelations should have time duration (user can cancel their booking, 3days after the booking).


NOTIFICATIONS (OPTIONAL)
- Sends Notification to users upon booking creation, update, cancellation and will remind the user before their bookings start (Our systems notification will be through email)


REPORTING & ANALYTICS 
-Booking Summary (Admin), will summarize booking data by room, date and user (All booking summary can be seen in other tabs, formatted in a list view per Room then if clicked the summary will be seen
- Analytics (Admin), generates report about all the room usage including total bookings, peak usages times (date and peak hours) and user activity (users most picked rooms/facility). Analytics can be displayed in the casual manner or in a chart for better admin visualizations.

SETTINGS
-Set Preferences, allows user to get preferences such as notification settings (they will be asked if they want to be notified via Gmail) and default booking duration (Add an option in which the user can put their default hrs. of duration, this can be seen in the users profile)


CALENDAR INTEGRATION (OPTIONAL)
- View Calendar, display bookings and allow Users to Book Rooms Directly from Calendar. (This can be displayed in the dashboard in which users can click dates vacant and book from there directly.) The calendar will work accordingly with the Booking management module


ADDITIONALS
- Cancellation of booking cannot be determined or not fixed since it will depend on the company, but in our development we can put am initial time duration of acceptable booking cancellation
- Admins are the meeting room owner, Users are the ones who will book the room, Super Admins are us
-Database tables (Tbd, preferred sep per module)
