<script lang="ts">
	import type { AuthorData } from "$lib";
	import Icon from "$lib/components/Icon.svelte";
	import LoadingSpinner from "$lib/components/LoadingSpinner.svelte";
	import StatCard from "$lib/components/StatCard.svelte";
	import { mdiDownload, mdiHeart, mdiEye, mdiPackageVariant, mdiSteam } from "@mdi/js";

	const { creator } : { creator: AuthorData | undefined } = $props()

	const avatarSize = 176;
	async function getNames(steam_id: string, steam_name: string): Promise<string> {
		let names: string[] = await (await fetch(`/api/creator_names/${steam_id}`)).json();
		names = names.filter(x => x != steam_name)
		if (names.length === 0) return "";
		if (names.length === 1) return names[0];
		return names.slice(0, -1).join(", ") + " or " + names[names.length - 1];
	}

</script>

<header class="panel">
	{#if creator}
		<div id="header-left" style="--avatar-size: {avatarSize}px">
			<div class="panel--wrapper">
				<img class="panel" id="creator-avatar" src={creator.steam_avatar} alt={creator.steam_name} width={avatarSize} height={avatarSize} />
			</div>
			<div id="header-body">
				<div id="creator-name">
					<h1>{creator.steam_name}</h1>
					<a href="https://steamcommunity.com/profiles/{creator.steam_id}" title="Creator Steam page">
						<Icon path={mdiSteam} />
					</a>
				</div>
				<div id="creator-meta">
					{#await getNames(creator.steam_id.toString(), creator.steam_name)}
						<LoadingSpinner size={16} />
					{:then names}
						{#if names}
							<span>Also known as:</span>
							<span>{names}</span>
						{/if}
					{/await}
				</div>
			</div>
		</div>
		<div id="stat-grid">
			<StatCard label="Mods Created" value={creator.total} icon={mdiPackageVariant} />
			<StatCard label="Total Downloads" value={creator.total_downloads} icon={mdiDownload} />
			<StatCard label="Total Favorites" value={creator.total_favorites} icon={mdiHeart} />
			<StatCard label="Total Views" value={creator.total_views} icon={mdiEye} />
		</div>
	{:else}
		<LoadingSpinner size=24 />
	{/if}
</header>

<style>
	header {
		display: flex;
		flex-wrap: wrap;
		justify-content: center;
		align-items: center;
		gap: 2rem 8rem;
		padding: clamp(1.5rem, 3vw, 2.5rem);
	}

	#header-left {
		display: grid;
		grid-template-columns: var(--avatar-size) 1fr;
		gap: clamp(1.5rem, 3vw, 3rem);
		align-items: center;
	}

	#header-left > .panel--wrapper {
		--panel-border: var(--icon-border-color);
	}

	#creator-name {
		font-family: "Andy";
		display: flex;
		align-items: baseline;
		gap: 1rem;
	}

	#header-body {
		display: flex;
		flex-direction: column;
		gap: 0.75rem;

		h1 {
			margin: 0;
		}
	}

	#creator-meta {
		color: rgba(255, 255, 255, 0.7);
		font-size: 1.1rem;
		text-wrap: pretty;

		& > :not(:first-child) {
			font-family: "Andy";
		}
	}

	#stat-grid {
		display: grid;
		grid-template-columns: repeat(2, 1fr);
		gap: 0.75rem;
		width: 100%;
		flex: 1;
		max-width: 60rem;
	}

	@media (max-width: 730px) {
		#header-left {
			display: flex;
			flex-direction: column;

			#creator-avatar {
				align-self: center;
			}
		}

		#header-body {
			align-items: center;
		}
	}

	@media (max-width: 555px) {
		#stat-grid {
			grid-template-columns: unset;
		}
	}
</style>
