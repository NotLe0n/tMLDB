<script>
	let {id, name, modCount, totalDownloads, avatar} = $props();
</script>

<a href="/creator/{id}">
	<div class="creator-card">
		<img class="creator-icon" src={avatar} alt={name} width="60px" height="60px">
		<div class="creator-info">
			<h3 class="creator-name">{name}</h3>
			<div class="creator-stats">
				<span>{modCount} {modCount == 1 ? "mod" : "mods"}</span>
				<span>{totalDownloads?.toLocaleString("en", {
					notation: "compact",
					compactDisplay: "short"
				}) ?? "unknown"} downloads</span>
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
		border: 1px solid var(--green3);
		background-color: var(--green3-bg);
		border-radius: 0.75rem;
		padding: 1rem;
		transition: border-color 0.2s, background-color 0.2s;
		cursor: pointer;

		&:hover {
			border-color: var(--highlight);
			background-color: var(--green3-hov);
		}
	}

	.creator-icon {
		border-radius: 50%;
		border: 2px solid var(--highlight);
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
		font-size: 1.1rem;
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
