import { type CampaignObject } from "@/api/client";

interface Props {
	campaigns: CampaignObject[];
}

export enum CampaignPriority {
	LOW = 0,
	MEDIUM = 1,
	HIGH = 2,
}

export default function CampaignsTable({ campaigns }: Props) {
	function getPriority(priority: CampaignPriority) {
		switch (priority) {
			case CampaignPriority.LOW:
				return <span className="badge badge-xs badge-outline badge-success">Low</span>;
			case CampaignPriority.MEDIUM:
				return <span className="badge badge-xs badge-outline badge-warning">Medium</span>;

			case CampaignPriority.HIGH:
				return <span className="badge badge-xs badge-outline badge-error">High</span>;
		}
	}

	return (
		<table className="table">
			<thead>
				<tr>
					<th>Nazwa</th>
				</tr>
			</thead>
			<tbody>
				{campaigns.map((campaign) => (
					<tr key={campaign.id}>
						<td>{campaign.name}</td>
						<td>{getPriority(campaign.priority)}</td>
					</tr>
				))}
			</tbody>
		</table>
	);
}
