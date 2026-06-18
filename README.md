# tMLDB - The tModLoader Database
tMLDB is a site which tracks and displays information about Terraria mods. 
- See the subscriber count, views and other statistics over time for any mod
- Search and find mod creators to see all their mods and aggregated statistics
- View a searchable, sortable list of all mods with filters for tags and versions
- Compare the stats of mods with eachother

# Gallery

|  |  |
|--|--|
|<img width="1920" height="959" alt="homepage" src="https://github.com/user-attachments/assets/11cac363-325a-4ed8-b95c-3adb5d3b22ad" />|<img width="1920" height="959" alt="mod view" src="https://github.com/user-attachments/assets/7dcadd43-2859-41d0-a84c-cdfdad6ca24f" />|
|<img width="1920" height="959" alt="mod graph" src="https://github.com/user-attachments/assets/7d87739d-7796-4bd6-a689-9f114361e491" />|<img width="1920" height="959" alt="mod list" src="https://github.com/user-attachments/assets/4930587d-c699-4b50-8da1-8daf99497a74" />|
|<img width="1920" height="959" alt="creator list" src="https://github.com/user-attachments/assets/23118965-7907-415b-b1a5-fe0365735e77" />|<img width="1920" height="959" alt="creator" src="https://github.com/user-attachments/assets/74a35ffe-917f-449d-85e6-dbef2b865678" />|
|<img width="1920" height="959" alt="creator graph" src="https://github.com/user-attachments/assets/c6ab8b7d-7f30-4e6f-bf16-1e76a4666e0b" />|<img width="1920" height="959" alt="compare mods" src="https://github.com/user-attachments/assets/38f7f719-5179-444c-b130-4f7927f1a842" />|
|  |  |


# Stack
- Frontend: Svelte / SvelteKit
- Backend API: ASP.NET
- Database Access: NpgSQL+Dapper

# Data source
The data for this site is sourced from two locations. 
Up-to-date data is fetched from [tMLAPIs](https://github.com/NotLe0n/tMLAPIs) (SteamAPI) for the `/mod`, `/creator` and `compare` pages. 
Historical data and mod list data is queried from the Database. The Historical data is updated daily, while the mod list is updated hourly.
