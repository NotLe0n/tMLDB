// place files you want to import through the `$lib` alias in this folder.
export const tmlapis = "https://tmlapis.le0n.dev/"

export const S_TO_YRS = 31556952;
export const S_TO_MTH = 2629746;
export const S_TO_DAY = 86400;
export const S_TO_HRS = 3600;
export const S_TO_MIN = 60;

export function msToDuration(value: number) {
	const duration = {
		years: Math.floor(value / S_TO_YRS),
		months: Math.floor(value / S_TO_MTH),
		days: Math.floor(value / S_TO_DAY),
		hours: Math.floor(value / S_TO_HRS),
		minutes: Math.floor(value / S_TO_MIN),
		seconds: Math.floor(value),
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

	if (duration.years > 0) return duration.years + " year" + (duration.years > 1 ? "s" : "")
	if (duration.months > 0) return duration.months + " month" + (duration.months > 1 ? "s" : "")
	if (duration.days > 0) return duration.days + " day" + (duration.days > 1 ? "s" : "")
	if (duration.hours > 0) return duration.hours + " hour" + (duration.hours > 1 ? "s" : "")
	if (duration.minutes > 0) return duration.minutes + " minute" + (duration.minutes > 1 ? "s" : "")
	if (duration.seconds === 0) return "0 seconds"
	return duration.seconds + " second" + (duration.seconds > 1 ? "s" : "")
}

export function formatNumber(value: number) {
	return new Intl.NumberFormat('en-US', { notation: 'standard' }).format(value)
}

export function formatNumberShort(value: number) {
	return new Intl.NumberFormat('en-US', { notation: 'compact' }).format(value)
}

export function formatBytes(value: number) {
	if (value / 1_000_000_000 > 1) {
		return `${value / 1_000_000_000} GB`
	}
	if (value / 1_000_000 > 1) {
		return `${value / 1_000_000} MB`
	}
	if (value / 1_000 > 1) {
		return `${value / 1_000} KB`
	}

	return `${value} Byte`
}

export type ModHistoryData = {
	dates: string[];
	subscriptions: number[];
	views: number[];
	favorited: number[];
	sessions: number[];
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

	subscriptions: number;
	subscriptions_total: number;
	favorited: number;
	favorited_total: number;
	followers: number;
	views: number;

	vote_data: {
		score: number;
		votes_up: number;
		votes_down: number;
	} | null;

	sessions: number;
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
	file_size: string;
};


export type ModListData = {
	mod_id: number;
	display_name: string;
	author: string;
	description: string;
	subscriptions_total: number;
	subscriptions: number;
	views: number;
	favorited_total: number;
	favorited: number;
	score: number;
	votes_up: number;
	votes_down: number;
	playtime: string;
	workshop_icon_url: string;
	time_updated: number;
	time_created: number;
	sessions: number;
	file_size: number;
}

export type AuthorData = {
	steam_id: string;
	steam_name: string;
	steam_avatar: string;
	mods: ModData[];
	total: number;
	total_subscriptions: number;
	total_favorites: number;
	total_views: number;
}

export type AuthorHistoryData = {
	dates: string[];
	lines: {
		label: number;
		data: number[];
		dates: string[];
	}[]
}

export type CreatorListData = {
	author_id: string;
	author_name: string;
	mod_count: number;
	subscriptions: number;
	views: number;
	favorites: number;
	avatar?: string;
};

export type SearchResult = {
	name: string,
	id: string
}

export const DATE_RANGES = ["Full", "Year", "4 Months", "Month", "2 Weeks", "Week"] as const;
export type DateRange = typeof DATE_RANGES[number];
