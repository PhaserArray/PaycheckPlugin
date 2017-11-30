# PaycheckPlugin
An unturned rocket plugin that adds the ability to give players different amounts of experience every X amount of seconds. It also features zones which apply multipliers to the experience, these can be defined globally or on a per paycheck basis.

---

### Commands

**Vector Note:** If a command takes a vector, it will round the result and for finding purposes, it will try to find the closest vector that's rounded form is an exact match. Any normal, square, angled or pointy brackets don't really matter, neither does whitespace (if surrounded with quotes).  
**Paycheck Name Note:** Paycheck names are case insensitive. A partial name is also acceptable. It will always return the match where the search string makes up the highest percentage of the paycheck name (for example: searching for paycheck `a` in a list of `apple,a124,paycheckname` will always return `a124`.  
**Node Name Note:** THe name can be a partial match, but it is case sensitive.  

**Name:** `nextpaycheck`   
**Alias:** `npay`  
**Permission:** `paychecks.commands.view`  
**Description:** Shows the amount of time until the next paycheck is given out.

**Name:** `createpaycheck`  
**Usage:** `[name] [experience] <paychecktocopyzonesfrom>`  
**Alias:** `cpay`  
**Permission:** `paychecks.commands.manage`  
**Description:** This command allows you to create a paycheck. You can specify a paycheck to copy zones from to make your life easier.

**Name:** `createpaycheckzone`  
**Usage:** `<paycheck> <node | x,y,z> [radius] [multiplier]`  
**Alias:** `cpayz`  
**Permission:** `paychecks.commands.manage`  
**Description:** This command creates a zone. There are multiple ways to use this command. It has to have the radius and multiplier as the last two arguments. The easiest way to use this is to simply provide the radius and multiplier, in which case, the zone will be created at the caller's location as a global zone. The first argument can either be a paycheck to add the zone to, in which case the location will be the caller's position, or it can be a vector or (optionally partial) node name, in this case the zone is global. Lastly, if all parameters are provided, the zone will be for a specific paycheck at the provided location.

**Name:** `listpaychecks`  
**Alias:** `lpay`  
**Permission:** `paychecks.commands.view`  
**Description:** Lists all paychecks and the amount of XP they give.

**Name:** `listpaycheckzones`  
**Usage:** `<paycheck>`  
**Alias:** `lpayz`  
**Permission:** `paychecks.commands.view`  
**Description:** Lists all paycheck zones. If a paycheck name is not provided, it will list all global zones.

**Name:** `deletepaycheck`   
**Usage:** `[name]`  
**Alias:** `dpay`  
**Permission:** `paychecks.commands.manage`  
**Description:** Deletes a paycheck with the provided name.

**Name:** `deletepaycheckzone`  
**Usage:** `<paycheck> [index | node | (x,y,z)]`  
**Alias:** `dpayz`  
**Permission:** `paychecks.commands.manage`  
**Description:** Deletes a paycheck zone. If the paycheck name is not provided, it will delete from the global zones. The zone can be selected by either its index from `/listpaycheckzones`, the zone's node name or its position.

---

### Configuration

#### Settings
`Interval` - How often the paychecks are distributed. This must be >0, also, do not set this too low in order to avoid any potential lag.  
`DisplayNotification` - Whether a message should be sent to the player when they get or fail to get their paycheck.  
`AllowMultipleMultipliers` - Whether multipliers should be multiplied together if a player is in multiple zones at once. If false, only the closest multiplier will be used.  
`AllowPaychecksWhenDead` - Whether a player can receive paychecks while dead. It is a good idea to keep this as false for AFK protection.  
`AllowPaychecksInSafezone` - Whether a player can receive paychecks while in a safezone. It is a good idea to keep this as false for AFK protection.  
`AllowMultiplePaychecks` - Whether a player can receive multiple paychecks. If true, they will be added together, if false, only the player's highest paycheck will be given out.  
`Paychecks` - List of paychecks.
`PaycheckZones` - List of global zones, these are applied to all paychecks.

#### Paychecks

Paychecks consist of 3 components.

`Name` - The name of the paycheck. The permission the player needs to get paid is based on this, it is `paycheck.paycheckname`.  
`Experience` - The amount of experience the paycheck gives out.  
`PaycheckZones` - List of zones specific to this paycheck. Anyone with a different paycheck will not be affected by these zones.  

#### Paycheck Zones

`Node` - The name of the node the zone is located at. This can be a partial name and is case sensitive. This can match more than one node on the map (e.g. `HQ` can match `Axis HQ` and `Allies HQ`)  you use a `Point`, delete this line.  
`Point` - The coordinates of where the zone is located. If you use node, set this to `<Point xsi:nil="true" />`. If you use both, only this will be used.
`Radius` - The distance this zone extends out to from the Node or Point.  
`Multiplier` - The multiplier that is applied to the paychecks of anyone within the zone.

---

### Localization

**Note:** Anything not prefixed with `command_` only gets shown if `DisplayNotification` is true.

`paycheck_zero_multiplier` - String shown when the player has a paycheck, but it is made zero by the current multipliers.  
`paycheck_given` - String shown when a player receives their paycheck. {0} is the amount of experience gained.  
`paycheck_notgiven` - String shown when a player could not receive their paycheck. This is usually shown due to a prevented overflow. {0} is the XP not given.  
`paycheck_dead` - Shown when dead players cannot receive paychecks and the player is dead.  
`paycheck_safezone` - Shown when players in safezones cannot receive paychecks and the player is in one.  
`command_list_paychecks` - String followed by a list of paychecks which are represented as {0}.  
`command_no_paychecks` - String shown when there are no paychecks.  
`command_default_no_zones` - String shown when there are no global zones.  
`command_paycheck_no_zones` - String shown when there are no zones for paycheck {0}.  
`command_list_default_zones` - String followed by a list of global zones which are represented as {0}.  
`command_list_paycheck_zones` - String followed by a list of a paycheck's zones which are represented as {1}. The name of the paycheck is {0}.  
`command_paycheck_deleted` - String shown when a paycheck is deleted, {0} is the name of the paycheck.  
`command_delete_zone_no_parse` - String shown when a zone could not be found when attempting to delete it.  
`command_invalid_out_of_bounds` - String shown when the index is out of bounds. {0} is the input index, {1} and {2} are the inclusive min and max.  
`command_removed_zone_default` - String shown when a global zone is removed. {0} is the zone location.  
`command_removed_zone_paycheck` - String shown when a paycheck zone is removed. {1} is the zone location and {0} is the paycheck name.  
`command_no_parse_experience` - String shown when the experience for create paycheck could not be parsed.  
`command_paycheck_created` - String shown when a paycheck is created. {0} is the paycheck name, {1} is the experience number.  
`command_no_console` - String shown when a command that can be used in the console is called in a way that the console can't call it (in this case, the command needs the caller's location if called in this way).  
`command_no_parse_multiplier` - String shown when a paycheck zone's multiplier parameter could not be parsed as a float. {0} is the input.  
`command_no_parse_radius` - String shown when a paycheck zone's radius parameter could not be parsed as a float. {0} is the input.  
`command_no_parse_location` - String shown when a paycheck's location could not be parsed as coordinates or a node. {0} is the input.  
`command_created_zone_default` - String shown when a global zone is created. {0} is the location, {1} is the radius and {2} is the multiplier.  
`command_created_zone_paycheck` - String shown when a paycheck zone is created. {0} is the paycheck name, {1} is the location, {2} is the radius and {3} is the multiplier.  
`command_no_parse_paycheck_or_location` - String shown when a parameter could not be parsed as either a paycheck, node or coordinates. {0} is the input.  
`command_time_to_next_paycheck_minutes` - String shown when a player checks the amount of time until the next paycheck and there are more than 60 seconds left. {0} is the minutes left and {1} is the seconds left.  
`command_time_to_next_paycheck` - String shown when a player checks the amount of time until the next paycheck and there are les than 60 seconds left. {0} is the seconds left.
