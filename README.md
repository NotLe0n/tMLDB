# tMLDB - The tModLoader Database
tMLDB is a site which tracks and displays information about Terraria mods. 
- See the subscriber count, views and other statistics over time for any mod
- Search and find mod creators to see all their mods and aggregated statistics
- View a searchable, sortable list of all mods with filters for tags and versions

# Gallery

|  |  |
|--|--|
|<img width="1920" height="959" alt="image" src="https://github.com/user-attachments/assets/c41bec85-d4c7-433d-a61c-82f690c7f6b2" />|<img width="1920" height="959" alt="image" src="https://github.com/user-attachments/assets/c9013178-c908-4f0b-8a32-64b2a1c81ef3" />|
|<img width="1920" height="959" alt="image" src="https://github.com/user-attachments/assets/e7073648-633a-46de-8903-ed0a08109805" />|<img width="1920" height="959" alt="image" src="https://github.com/user-attachments/assets/ed9783bf-c73c-4eae-8d47-a2f15366a231" />|
|<img width="1920" height="959" alt="image" src="https://github.com/user-attachments/assets/0275a657-ff6d-4750-b3b5-c7b06ef236d0" />|<img width="1920" height="959" alt="image" src="https://github.com/user-attachments/assets/65858831-2df0-4248-807c-23e6dbdae630" />|
|<img width="1920" height="959" alt="image" src="https://github.com/user-attachments/assets/d7359d2d-d07d-4d3b-8d72-0f9b7e4b6d98" />|
|  |  |

# Stack
- Frontend: Svelte / SvelteKit
- Backend API: ASP.NET
- Database Access: NpgSQL+Dapper

# Data source
The data for this site is sourced from two locations. 
Up-to-date data is fetched from [tMLAPIs](https://github.com/NotLe0n/tMLAPIs) (SteamAPI) for the `/mod` and `/creator` pages. 
Historical data and mod list data is queried from the Database. The Historical data is updated daily, while the mod list is updated hourly.
