"use client";
import CampaignsTable from "@/components/CampaignTable";
import { CampaignObject, client, UserType } from "@/api/client";
import { useContext, useEffect, useMemo, useState } from "react";
import { UserContext } from "../context/auth";
import { CreateCampaign } from "@/components/CreateCampaign";

export default function Campaigns() {
	const currentUser = useContext(UserContext);

	const [campaigns, setCampaigns] = useState<CampaignObject[]>([]);
	const [isLoading, setLoading] = useState(true);
	const [modalOpen, setModalOpen] = useState(false);

	async function fetchCampaigns() {
		setLoading(true);
		const { data, error } = await client.GET("/api/campaigns");
		setLoading(false);

		if (error) return;

		setCampaigns(data);
	}

	useEffect(() => {
		fetchCampaigns();
	}, []);

	const isManager = useMemo(() => currentUser?.userType === UserType.HIRING_MANAGER, [currentUser]);

	return (
		<>
			<div>
				<div className="flex justify-between">
					<div>
						<h2 className="font-extrabold text-3xl mb-4">Kampanie</h2>
					</div>
					{isManager && (
						<button className="btn btn-success text-white" onClick={() => setModalOpen(true)}>
							Utwórz nową
						</button>
					)}
				</div>
				<div className="overflow-x-auto mt-20">
					{isLoading && <span className="loading loading-dots loading-xl"></span>}
					{!isLoading && <CampaignsTable campaigns={campaigns} />}
				</div>
			</div>
			<CreateCampaign open={modalOpen} setOpen={setModalOpen} done={fetchCampaigns} />
		</>
	);
}
