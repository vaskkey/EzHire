"use client";
import CampaignsTable from "@/components/CampaignTable";
import { CampaignObject, client } from "@/api/client";
import { useEffect, useState } from "react";

export default function Campaigns() {
	const [campaigns, setCampaigns] = useState<CampaignObject[]>([]);
	const [isLoading, setLoading] = useState(true);

	useEffect(() => {
		client.GET("/api/campaigns").then(({ data, error }) => {
			if (error) return;

			setCampaigns(data);
			setLoading(false);
		});
	}, []);

	return (
		<div>
			<div className="flex justify-between">
				<div>
					<h2 className="font-extrabold text-3xl mb-4">Kampanie</h2>
				</div>
			</div>
			<div className="overflow-x-auto mt-20">
				{isLoading && "Loading"}
				{!isLoading && <CampaignsTable campaigns={campaigns} />}
			</div>
		</div>
	);
}
