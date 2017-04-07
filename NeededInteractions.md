# Which interactions with database do we need?
  ## Venue Interactons
    -Add new Venue.
    -Add new Resources to a Venue
    -Edit any information about a Venue
    -Search Venues : -based on tags
                     -based on Name
                     -based on Location
                     -some important other info...
    -Delete Venues
  ## Resource Interactions
    -Resources must be edited at any time, owner can rearrange his venue...
    -Change status of a resource, as fast as possible(employees must be fast)
    -Create reservation for a Resource
    -Delete reservation if reservation is expired(what if customer doesn't come when he reserved, we must delete that reservation then)
    -Show all reservations(reservations will expire so we will check out reservations inside few days)
      (possible trollers - BEWARE!! - they just make bunch of reservations for example a week.)
      -we need to either create rules for reservations or charge small fee..
    -We must effiecently show statues of objects including reservations within an hour or some other short period of time
    - if user asks more detaily about a venue - show them resources
    
    ### Reservations
      -must have some sort of secret code - veryfying that a specific user made reservation for a specific resource
      -a reservaton is tied to one user
  
  ## Users
    -guests? - useful for : -suggestions based on latest searches...
    -Owners - can add new Employees for a Venue
            - we must somehow make a owner interact with his venues easily
    -Employees - must easily change status of a resource and fast(speed is key...)
    
    
    
much more to come...
    
    
    
    
                     
