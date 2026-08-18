<script>
	let {id, name, modCount, totalSubscriptions, avatar} = $props();
</script>

<a href="/creator/{id}">
	<div class="creator-card panel hover-highlight">
		<div class="panel--wrapper">
			<img class="panel creator-icon" src={avatar} alt={name} width="60px" height="60px">
		</div>
		<div class="creator-info">
			<h3 class="creator-name">{name}</h3>
			<div class="creator-stats">
				<span>{modCount} {modCount == 1 ? "mod" : "mods"}</span>
				<span>{totalSubscriptions?.toLocaleString("en", {
					notation: "compact",
					compactDisplay: "short"
				}) ?? "unknown"} subscriptions</span>
			</div>
		</div>
	</div>
</a>

<style>
	.creator-card {
		container-type: inline-size;
		display: flex;
		align-items: center;
		gap: 1rem;
		background-color: var(--panel-col2);
		border-radius: 0.75rem;
		padding: 1rem;
		cursor: pointer;
	}

	.creator-card > .panel--wrapper {
		--panel-border: var(--icon-border-color);
	}

	.creator-info {
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
	}

	.creator-name {
		width: 10rem;
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
		padding-bottom: 0.15rem;
		margin: 0;
		font-family: Andy;
	}

	.creator-stats {
		display: flex;
		gap: 0.25rem 1rem;
		font-size: 0.9rem;
		opacity: 0.8;

		@container (width <= 18rem) {
			flex-direction: column;
		}
	}
</style>
