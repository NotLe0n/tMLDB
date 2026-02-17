<script lang="ts">
	import { formatDate, formatDuration, type ModData, type ModListData } from "$lib";
	import { mdiClock, mdiClockEdit, mdiClockPlus, mdiDownload, mdiEye, mdiStar, mdiThumbDown, mdiThumbUp } from "@mdi/js";
	import ModIcon from "./ModIcon.svelte";
	import bbobHTML from '@bbob/html'
	import presetHTML5 from '@bbob/preset-html5'
    import Icon from "./Icon.svelte";

	const { mod }: { mod: ModListData | ModData } = $props()
	
	// Handle vote data differences between ModData and ModListData
	const score = $derived('score' in mod ? mod.score : mod.vote_data?.score ?? 0)
	const votesUp = $derived('votes_up' in mod ? mod.votes_up : mod.vote_data?.votes_up ?? 0)
	const votesDown = $derived('votes_down' in mod ? mod.votes_down : mod.vote_data?.votes_down ?? 0)
	
	const descriptionHTML = $derived(
		bbobHTML(
			mod.description?.substring(0, 800).replace(/^(\[h1|\[url|\[img).*$/gm, "").trim() ?? "", 
			presetHTML5(), {
				stripTags: true
			}
		)
	)

	function getVoteRatio(up: number, down: number) {
		if (up === 0 && down === 0) {
			return 50;
		}
		return (up / (up + down)) * 100;
	}

	const voteRatio = $derived(getVoteRatio(votesUp, votesDown))
	const timeCreated = $derived(formatDate(mod.time_created))
	const timeUpdated = $derived(formatDate(mod.time_updated))
	const iconSize = 128
</script>

<a href={`/mod/${mod.mod_id}`} class="link-container" style="--icon-size: {iconSize}px">
	<div class="modlist-card">
		<div class="mod-icon">
			<ModIcon src={mod.workshop_icon_url} name={mod.display_name} size={iconSize} />
		</div>
		<div class="content">
			<div class="header">
				<h3>{mod.display_name}</h3>
				<small>by {mod.author}</small>
			</div>
			<div class="description">
				{@html descriptionHTML}
			</div>
		</div>
		<div class="divider"></div>
		<div class="numbers">
			<div class="non-vote">
				<div class="stat-item time-stat">
					<Icon path={mdiClockPlus} size=16 />
					<span>Created on</span>
					<span>{timeCreated}</span>
				</div>
				<div class="stat-item time-stat">
					<Icon path={mdiClockEdit} size=16 />
					<span>Updated on</span>
					<span>{timeUpdated}</span>
				</div>

				<div class="stat-item" title="Downloads">
					<Icon path={mdiDownload} size=16 viewBox='5 0 15 19' />
					<span class="stat-value">{mod.downloads_total.toLocaleString()}</span>
				</div>
				<div class="stat-item" title="Views">
					<Icon path={mdiEye} size=16 />
					<span class="stat-value">{mod.views.toLocaleString()}</span>
				</div>
				<div class="stat-item" title="Favorites">
					<Icon path={mdiStar} size=16 />
					<span class="stat-value">{mod.favorited.toLocaleString()}</span>
				</div>
				<div class="stat-item" title="Cumulative Playtime">
					<Icon path={mdiClock} size=16 />
					<span class="stat-value">{formatDuration(Number.parseInt(mod.playtime))}</span>
				</div>
			</div>
			<div class="vote-stats">
				<div class="stat-item" title="Upvotes">
					<Icon path={mdiThumbUp} size=16 />
					<span class="stat-value">{votesUp.toLocaleString()}</span>
				</div>
				<div class="stat-item" title="Downvotes">
					<Icon path={mdiThumbDown} size=16 />
					<span class="stat-value">{votesDown.toLocaleString()}</span>
				</div>
				<div 
					class="rating-bar" 
					style="--score: {voteRatio}%" 
					title="Vote Ratio: {voteRatio.toFixed(2)}%"
				></div>
			</div>
		</div>
	</div>
</a>

<style>
	.link-container {
		container-type: inline-size;
		display: flex;
	}

	.modlist-card {
		display: grid;
		grid-template-columns: var(--icon-size) auto 2px auto;
		grid-template-rows: var(--icon-size);
		width: 100%;
		gap: 1rem;
		padding: 1rem;
		background-color: var(--tertiary-bg);
		border-radius: 1rem;
		transition: background-color 0.2s;
		align-items: center;

		&:hover {
			background-color: var(--tertiary-hov);
		}

		@container (width < 1000px) {
			grid-template-columns: var(--icon-size) auto;
			grid-template-rows: var(--icon-size) 2px auto;

			.numbers {
				grid-template-columns: 25rem minmax(10rem, 22rem);
				grid-template-rows: auto;
				
				gap: 0.5rem 4rem;
				align-items: center;
				justify-content: space-between;
			}
		}

		@container (700px <= width < 1000px) {
			.divider, .numbers {
				grid-column: span 2;
			}
		}

		@container (width < 700px) {
			grid-template-columns: 1fr;
			.mod-icon {
				display: none;
			}
			.numbers {
				grid-template-columns: 1fr;
				grid-template-rows: auto auto;
			}
		}

		@container (width < 400px) {
			grid-template-rows: auto auto;
			.divider, .description {
				display: none;
			}

			.non-vote {
				grid-template-columns: auto auto;
				grid-template-rows: repeat(4, 1rem);
				justify-content: space-between;
			}

			.header {
				flex-direction: column;
			}
		}
	}

	.divider {
		background-color: var(--tertiary);
		border-radius: 10px;
		height: 100%;
	}

	.content {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		min-width: 0;
		height: 100%;
	}

	.header {
		display: flex;
		gap: 0.25rem 1rem;
		align-items: baseline;

		small {
			color: rgb(from white r g b / 60%)
		}
	}

	.description {
		font-size: 0.9rem;
		line-height: 1.4;
		
		background: linear-gradient(to bottom, white 10%, transparent 100%);
		color: transparent;
		background-clip: text;
		overflow: hidden;
	}

	.numbers {
		height: 100%;
		display: grid;
		grid-template-rows: 1fr 1fr;
		align-items: center;
		justify-content: space-evenly;
	}

	.non-vote {
		display: grid;
		grid-template-columns: repeat(4, 1fr);
		grid-template-rows: 1fr 1fr;
		gap: 0.25rem 1rem;

		.time-stat {
			grid-column: span 2;
		}
	}

	.stat-item {
		display: flex;
		gap: .5rem;
		border-radius: 0.5rem;
		font-size: 0.8rem;
		align-items: center;
	}

	.stat-value {
		font-weight: 600;
		color: var(--primary);
	}

	.vote-stats {
		display: grid;
		grid-template-columns: auto auto;
		justify-content: space-between;
	}

	.rating-bar {
		width: 100%;
		height: 5px;
		border-radius: 20px;
		margin-top: 5px;
		background: linear-gradient(90deg,
			var(--primary) 0%,
			var(--primary) var(--score),
			rgba(235, 38, 38, 1) var(--score),
			rgba(235, 38, 38, 1) 100%
		);
		grid-column: span 2;
	}
</style>