<script lang="ts">
	import LinkButton from '$lib/components/LinkButton.svelte'
	import ModIcon from '$lib/components/ModIcon.svelte'
	import ModTag from './ModTag.svelte'
	import ModSocialsList from './ModSocialsList.svelte'
	import StatCard from '$lib/components/StatCard.svelte'
	import VotingCard from './VotingCard.svelte'
	import { mdiDownload, mdiHeart, mdiClockOutline, mdiEye, mdiChevronDown } from '@mdi/js'
	import { formatDate, formatDuration, type ModData } from '$lib'
	import LoadingSpinner from '$lib/components/LoadingSpinner.svelte';
    import Icon from '$lib/components/Icon.svelte';

	const { mod }: { mod: ModData | undefined } = $props()

	const modIconSize = 176
	let showMoreInfo = $state(false)

</script>

<header class="panel">
	{#if mod}
		<div id="header-left" style="--icon-size: {modIconSize}px">
			<div id="mod-icon">
				<ModIcon src={mod.workshop_icon_url} name={mod.display_name} size={modIconSize} />
			</div>

			<div id="header-body">
				<div id="mod-core">
					<div id="mod-name">
						<h1>{mod.display_name}</h1>
						<button onclick={() => showMoreInfo = !showMoreInfo} title="show more information">
							<Icon path={mdiChevronDown} size={16} />
						</button>
					</div>
					<div id="creator">
						<a id="creator-link" href="/creator/{mod.author_id}">by {mod.author}</a>
						<ModSocialsList socials={mod.socials} mod_id={mod.mod_id} />
					</div>
				</div>
				{#if showMoreInfo}
					<div id="mod-internals">
						<span>Mod ID: {mod.mod_id}</span>
						<span>Internal name: {mod.internal_name}</span>
					</div>
				{/if}
				<div id="mod-meta">
					<span>Created {formatDate(mod.time_created)}</span>
					<span class="dot">•</span>
					<span>Updated {formatDate(mod.time_updated)}</span>
					<span class="dot">•</span>
					<span>{mod.num_versions} versions</span>
				</div>
			</div>

			<div id="mod-homepage">
				{#if mod.homepage}
					<LinkButton link={mod.homepage} title="Homepage" />
				{/if}
			</div>
			<div id="mod-tags">
				{#each mod.tags as tag}
					<ModTag tag={tag} />
				{/each}
			</div>
		</div>
	{:else}
		<LoadingSpinner size=24 />
	{/if}
	<div id="header-right">
		<div id="stat-grid">
			{#if mod}
				<StatCard label="Downloads" value={mod.downloads_total} icon={mdiDownload} />
				<StatCard label="Favorites" value={mod.favorited} icon={mdiHeart} />
				<StatCard label="Views" value={mod.views} icon={mdiEye} />
				<StatCard label="Combined Playtime" value={formatDuration(Number.parseInt(mod.playtime))} icon={mdiClockOutline } />
				{#if mod.vote_data}
					<VotingCard votes_up={mod.vote_data.votes_up} votes_down={mod.vote_data.votes_down} score={mod.vote_data.score} />
				{/if}
			{:else}
				<LoadingSpinner size=24 />
			{/if}
		</div>
	</div>
</header>

<style>
	header {
		display: grid;
		grid-template-columns: 6fr 5fr;
		gap: 2rem;
		padding: clamp(1.5rem, 3vw, 2.5rem);
	}

	#header-left {
		display: grid;
		grid-template-columns: var(--icon-size) 1fr;
		grid-template-rows: auto 1fr;
		gap: .5rem clamp(1.5rem, 3vw, 3rem);
		align-items: center;
		max-width: 45rem;
	}

	#header-body {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
	}

	#mod-name {
		display: flex;
		gap: 1rem;
		align-items: center;
		& > h1 {
			font-family: "Andy";
		}
		& > button {
			top: -4px;
		}
	}

	#mod-internals {
		display: flex;
		gap: 2rem;
		color: rgba(255, 255, 255, 0.7);
	}

	#creator {
		display: flex;
		align-items: center;
		gap: 0.75rem;
	}

	#creator-link {
		font-family: "Andy";
		text-decoration: none;
		color: rgba(255, 255, 255, 0.85);
		transition: color 0.2s ease;

		&:hover {
			color: var(--yellow);
		}
	}

	#mod-meta {
		display: flex;
		gap: 1rem;
		color: rgba(255, 255, 255, 0.7);
		font-size: 0.9rem;
	}

	#mod-homepage {
		display: grid;
	}

	#mod-tags {
		display: flex;
		align-items: center;
		flex-wrap: wrap;
		gap: .2rem;
	}

	#header-right {
		display: flex;
		align-items: center;
		justify-content: center;
	}

	#stat-grid {
		display: grid;
		grid-template-columns: repeat(2, 1fr);
		gap: 0.75rem;
		width: 100%;
	}

	@media (min-width: 555px) {
		:global(.voting-card) {
			grid-column: span 2;
		}
	}

	@media (max-width: 1400px) {
		header {
			grid-template-columns: 1fr;
		}

		#stat-grid {
			grid-template-columns: repeat(3, 1fr);
		}
	}

	@media (max-width: 730px) {
		#header-left {
			display: flex;
			flex-direction: column;
			align-items: stretch;

			#mod-icon {
				align-self: center;
			}
		}

		#mod-name, #creator, #mod-meta, #mod-internals {
			justify-content: center;
		}

		#stat-grid {
			grid-template-columns: repeat(2, 1fr);
		}
	}

	@media (max-width: 555px) {
		#stat-grid {
			grid-template-columns: 1fr;
		}
	}
</style>
