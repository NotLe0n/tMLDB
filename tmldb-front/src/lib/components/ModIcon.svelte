<script>
	let { src, name, size } = $props()
</script>

<img {src} alt={name + " icon"} title={name} width={size} height={size}>

<style>
	img {
		filter: drop-shadow(2px 0 0 var(--icon-border-color))
			drop-shadow(0 2px 0 var(--icon-border-color))
			drop-shadow(-2px 0 0 var(--icon-border-color))
			drop-shadow(0 -2px 0 var(--icon-border-color));
	}
</style>
