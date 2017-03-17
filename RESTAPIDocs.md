# Rest api methode and params list:
* apiV0:
  * ``` FindVenues ``` - Return Venues that match wanted parametars, if there isn't any param, return all(or first, lets say 40 Venues):
    *  ``` Type ``` - Venue type (e.g. Bar, ...)
    *  ``` Name ``` - Name of the Venue 
    *  ``` Location ``` - Location of Venue
    *  ``` lossy ``` - search parametars must match exacly to the real values (e.g. if Name is "Havana" and lossy is "false" the item named "Havanna" will not be returned)
    *  ``` GeneralSearch ``` - if you want to search by any parametar (e.g. by owner name, location, ..)
    *  ``` NumberOfResults ``` - max number of items to return (deff = 40, for now)
    *  ``` SortBy ``` - sort by parametar(e.g. "Name", "Type", ...)
  * ``` VenueReservations ``` - Return Reservations for a Venue
    *  ``` VenueID ``` - Required, throw error if not specified
    *  ``` DateTimeFrom ``` - Reservations from
    *  ``` DateTimeTO ``` - Reservations to
    *  ``` Whos ``` -  Reservations by specific user
    *  ``` NumberOfResults ``` - max number of items to return (deff = 40, for now)
    *  ``` SortBy ``` - sort by parametar(e.g. "Name", "Type", ...)

### For now this will do, if you want to add something else, add it!