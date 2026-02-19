<script lang="ts">
	import type { CreatorListData } from "$lib";
	import { mdiDownload, mdiEye, mdiPackageVariant, mdiStar } from "@mdi/js";
	import Icon from "$lib/components/Icon.svelte";

	const { creator }: { creator: CreatorListData } = $props();
</script>

<a href={`/creator/${creator.author_id}`}>
	<div class="creator-list-card" id={creator.author_name}>
		<h3>{creator.author_name}</h3>
		<div class="card-content">
			<img src={creator.avatar} alt={creator.author_name}>
			<div class="stats-grid">
				<div class="stat-item" title="Mod Count">
					<Icon path={mdiPackageVariant} size=18 />
					<span class="stat-label">Mods</span>
					<span class="stat-value">{creator.mod_count.toLocaleString()}</span>
				</div>
				<div class="stat-item" title="Total Downloads">
					<Icon path={mdiDownload} size=18 viewBox='5 0 15 19' />
					<span class="stat-label">Downloads</span>
					<span class="stat-value">{creator.downloads.toLocaleString()}</span>
				</div>
				<div class="stat-item" title="Total Favorites">
					<Icon path={mdiStar} size=18 />
					<span class="stat-label">Favorites</span>
					<span class="stat-value">{creator.favorites.toLocaleString()}</span>
				</div>
				<div class="stat-item" title="Total Views">
					<Icon path={mdiEye} size=18 />
					<span class="stat-label">Views</span>
					<span class="stat-value">{creator.views.toLocaleString()}</span>
				</div>
			</div>
		</div>
	</div>
</a>

<style>
	.creator-list-card {
		display: grid;
		grid-template-rows: auto 1fr;
		gap: 0.15rem;
		padding: 1rem;
		background-color: var(--tertiary-bg);
		border-radius: 1rem;
		transition: background-color 0.2s, border-color 0.2s;
		container-type: inline-size;

		&:hover {
			background-color: var(--tertiary-hov);
			border-color: var(--highlight);
		}
	}

	img {
		width: 80px;
		height: 80px;
		border-radius: 1rem;
		border: 2px solid var(--primary);
	}

	.card-content {
		display: grid;
		grid-template-columns: 80px 1fr;
		gap: 1.5rem;
		justify-content: center;
	}

	h3 {
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
		padding-bottom: 0.33rem;
	}

	.stats-grid {
		display: grid;
		grid-template-rows: repeat(4, 1fr);
		gap: 0.2rem;
	}

	.stat-item {
		display: flex;
		align-items: center;
		gap: 0.5rem;
		font-size: 0.9rem;
	}

	.stat-label {
		color: rgba(255, 255, 255, 0.7);

		@container (width < 300px) {
			display: none;
		}
	}


	.stat-value {
		font-weight: 600;
		color: var(--highlight);
	}
</style>
