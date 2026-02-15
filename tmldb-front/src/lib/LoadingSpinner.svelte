<script>
	let { size, text = "Loading..." } = $props()
</script>

<div class="loading-container">
	<span class="loader" style="--size: {size}px"></span>
	<span class="loader-text">{text}</span>
</div>

<style>
	.loading-container {
		display: flex;
		gap: 1rem;
		justify-content: center;
	}

	.loader {
		width: var(--size);
		height: var(--size);
		border: calc(var(--size) / 5) solid #FFF;
		border-bottom-color: transparent;
		border-radius: 50%;
		display: inline-block;
		box-sizing: border-box;
		animation: rotation 700ms linear infinite;
	}

	.loader-text {
		align-content: center;
	}
	
	@keyframes rotation {
		0% {
			transform: rotate(0deg);
		}
		100% {
			transform: rotate(360deg);
		}
	}
</style>