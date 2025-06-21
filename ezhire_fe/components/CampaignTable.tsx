import { CampaignPriority, type CampaignObject } from "@/api/client";
import Link from "next/link";

interface Props {
	campaigns: CampaignObject[];
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
					<th>Priorytet</th>
				</tr>
			</thead>
			<tbody>
				{campaigns.map((campaign) => (
					<tr key={campaign.id}>
						<td>
							<Link href={`/campaigns/${campaign.id}/postings`}>{campaign.name}</Link>
						</td>
						<td>{getPriority(campaign.priority)}</td>
					</tr>
				))}
			</tbody>
		</table>
	);
}
