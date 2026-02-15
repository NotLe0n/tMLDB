<script>
	import SvgIcon from '@jamescoyle/svelte-icon';

	const { path, ...others } = $props()
</script>

<SvgIcon type="mdi" {path} class="mdi-icon" {...others} />

<style>
	:global(a>.mdi-icon:hover path) {
		transition: fill .25s;
		fill: var(--highlight) !important;
	}
</style>