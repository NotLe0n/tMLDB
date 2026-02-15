// place files you want to import through the `$lib` alias in this folder.
export const MS_TO_YRS = 31556952000;
export const MS_TO_MTH = 2629746000;
export const MS_TO_DAY = 86400000;
export const MS_TO_HRS = 3600000;
export const MS_TO_MIN = 60000;
export const MS_TO_SEC = 1000;

export function msToDuration(value: number) {
	const duration = {
		years: Math.floor(value / MS_TO_YRS),
		months: Math.floor(value / MS_TO_MTH),
		days: Math.floor(value / MS_TO_DAY),
		hours: Math.floor(value / MS_TO_HRS),
		minutes: Math.floor(value / MS_TO_MIN),
	};

	return duration;
}

export function formatDate(value: number) {
	const date = new Date(value * 1000);
	return new Intl.DateTimeFormat('en-US', {
		year: 'numeric',
		month: 'short',
		day: '2-digit'
	}).format(date);
};

export function formatDuration(value: number) {
	const duration = msToDuration(value)
	
	if (duration.years > 0) return duration.years + " years"
	if (duration.months > 0) return duration.months + " months"
	if (duration.days > 0) return duration.days + " days"
	if (duration.hours > 0) return duration.hours + " hours"
	return duration.minutes + " minutes"
}

export type ModHistoryData = {
	dates: string[];
	downloads: number[];
	views: number[];
	favorited: number[];
	playtime: number[];
	votes_up: number[];
	votes_down: number[];
	time_updated: number[];
	version: string[];
};

export type ModData = {
	display_name: string;
	internal_name: string;
	mod_id: number;
	author: string;
	author_id: number;
	modside: string;
	homepage: string;

	versions: {
		mod_version: string;
		tmodloader_version: string;
	}[];

	tmodloader_version: string;
	version: string;
	mod_references: string;
	num_versions: number;

	tags: {
		tag: string;
		display_name: string;
	}[] | null;

	time_created: number;
	time_updated: number;
	workshop_icon_url: string;

	children: number[] | null;

	downloads_total: number;
	favorited: number;
	followers: number;
	views: number;

	vote_data: {
		score: number;
		votes_up: number;
		votes_down: number;
	} | null;

	playtime: string;
	num_comments: number;
	description: string | null;

	socials: {
		youtube: string | null;
		twitter: string | null;
		facebook: string | null;
		reddit: string | null;
		sketchfab: string | null;
	} | null;
};


export type ModListData = {
	mod_id: number;
	display_name: string;
	author: string;
	description: string;
	downloads_total: number;
	views: number;
	favorited: number;
	score: number;
	votes_up: number;
	votes_down: number;
	playtime: string;
	workshop_icon_url: string;
	time_updated: number;
	time_created: number;
}

export type AuthorData = {
	steam_id: string;
	steam_name: string;
	steam_avatar: string;
	mods: ModData[];
	total: number;
	total_downloads: number;
	total_favorites: number;
	total_views: number;
}

export type AuthorHistoryData = {
	dates: string[];
	lines: {
		label: number;
		data: number[];
	}[]
}

export type CreatorListData = {
	author_id: string;
	author_name: string;
	mod_count: number;
	downloads: number;
	views: number;
	favorites: number;
	avatar?: string;
};

export type SearchResult = {
	name: string,
	id: string
}


export const tmlapis = "https://tmlapis.le0n.dev/"