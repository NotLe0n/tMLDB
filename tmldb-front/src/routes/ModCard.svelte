<script lang="ts">
	import bbobHTML from '@bbob/html'
	import presetHTML5 from '@bbob/preset-html5'
	import ModIcon from '$lib/components/ModIcon.svelte';

	type TopModsResponse = { display_name: string, internal_name: string, description: string, icon: string }
	let {mod}: { mod: TopModsResponse } = $props();

	const descriptionHTML = $derived(
		bbobHTML(
			mod.description.substring(0, 440).replace(/^(\[h1|\[url|\[img).*$/gm, "").trim(), 
			presetHTML5(), {
				stripTags: true
			}
		)
	)
</script>

<a href="/mod/{mod.internal_name}">
	<div class="mod-card">
		<ModIcon src={mod.icon} name={mod.display_name} size="176px" />
		<div class="mod-card-content">
			<h3 class="mod-name">{mod.display_name}</h3>
			<div class="mod-card-description">
				{@html descriptionHTML}
			</div>
		</div>
	</div>
</a>

<style>
	a {
		text-decoration: none;
	}

	.mod-card {
		display: grid;
		grid-template-rows: 176px 8rem;
		place-items: center;
		gap: 1rem;

		background-color: var(--tertiary-bg);
		border: 1px solid var(--primary);
		border-radius: 0.75rem;
		padding: 1rem;

		transition: transform 0.2s, box-shadow 0.2s;
		cursor: pointer;

		&:hover {
			border-color: var(--highlight);
			transform: translateY(-4px);
			box-shadow: 0 4px 12px 0 rgb(from var(--secondary) r g b / 0.3);
			background-color: var(--tertiary-hov);
		}
	}

	:global(.mod-card-description h2) {
		font-size: 1rem;
	}

	.mod-card-content {
		display: flex;
		flex-direction: column;
		width: 100%;
		gap: 0.25rem;
		overflow: clip;
	}

	.mod-name {
		margin: 0;
		font-size: 1.2rem;
		color: var(--highlight);
	}

	.mod-card-description {
		margin: 0;
		font-size: 0.8rem;
		opacity: 0.8;
		line-height: 1.4;
		text-overflow: ellipsis;
		text-wrap-style: pretty;
		overflow-wrap: anywhere;
		height: 5rem;

		background: linear-gradient(to bottom, white 50%, transparent);
		color: transparent;
		background-clip: text;
	}
</style>