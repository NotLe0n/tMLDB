<script>
	let { link, title } = $props()
</script>

<a class="panel hover-highlight" href={link}>{title}</a>

<style>
a {
	text-decoration: none;
	background-color: var(--panel-bg);
	padding: 0.75rem 1.5rem;
	font-size: 1rem;
	font-weight: 600;
	cursor: pointer;
	text-align: center;
}
</style>
