<script>
	let { link, title } = $props()
</script>

<a href={link}>{title}</a>

<style>
a {
	text-decoration: none;
	background-color: transparent;
	color: var(--highlight);
	border: 2px solid var(--highlight);
	padding: 0.75rem 1.5rem;
	border-radius: 0.5rem;
	font-size: 1rem;
	font-weight: 600;
	cursor: pointer;
	transition: background-color  0.3s, color 0.3s;
	text-align: center;

	&:hover {
		background-color: var(--highlight);
		color: #0a0a0a;
	}
}
</style>