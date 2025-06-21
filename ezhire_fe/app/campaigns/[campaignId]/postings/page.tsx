"use client";

import { client, CampaignObject, CampaignPriority, PostingStatus, UserType } from "@/api/client";
import { UserContext } from "@/app/context/auth";
import { CreatePosting } from "@/components/CreatePosting";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useContext, useEffect, useMemo, useState } from "react";

export default function Postings({ params }: { params: Promise<{ campaignId: number }> }) {
	const currentUser = useContext(UserContext);

	const [campaign, setCampaign] = useState<CampaignObject>({
		id: 0,
		name: "",
		priority: CampaignPriority.LOW,
		postings: [],
		createdAt: "",
		updatedAt: "",
	});
	const [campaignId, setCampaignId] = useState<number>(0);
	const [isLoading, setLoading] = useState(true);
	const [modalOpen, setModalOpen] = useState(false);

	const router = useRouter();

	const badgeStatus = new Map([
		[CampaignPriority.LOW, "badge-success"],
		[CampaignPriority.MEDIUM, "badge-warning"],
		[CampaignPriority.HIGH, "badge-error"],
	]);

	const statusName = new Map([
		[CampaignPriority.LOW, "Niski"],
		[CampaignPriority.MEDIUM, "Średni"],
		[CampaignPriority.HIGH, "Wysoki"],
	]);

	const postingStatus = new Map([
		[PostingStatus.OPEN, "badge-success"],
		[PostingStatus.CLOSED, "badge-error"],
	]);

	const postingStatusName = new Map([
		[PostingStatus.OPEN, "Otwarte"],
		[PostingStatus.CLOSED, "Zamknięte"],
	]);

	const isManager = useMemo(() => currentUser?.userType === UserType.HIRING_MANAGER, [currentUser]);

	async function fetchCampaign() {
		setLoading(true);
		const prms = await params;
		setCampaignId(prms.campaignId);

		const { data, error } = await client.GET("/api/campaigns/{id}", { params: { path: { id: prms.campaignId } } });

		setLoading(false);

		if (error) {
			router.replace("/campaigns");
			return;
		}

		setCampaign(data);
	}

	useEffect(() => {
		fetchCampaign();
	}, []);

	return (
		<div>
			{isLoading && ""}
			{!isLoading && (
				<>
					<div className="flex justify-between">
						<div>
							<h2 className="font-extrabold text-3xl mb-4">{campaign.name}</h2>
							<div className="flex flex-row gap-4">
								<h5 className="text-xs flex gap-3 items-center">
									Priorytet:
									<span
										className={`badge badge-sm badge-outline ${badgeStatus.get(campaign.priority) ?? CampaignPriority.LOW}`}
									>
										{statusName.get(campaign.priority)}
									</span>
								</h5>
							</div>
						</div>
						{isManager && (
							<button className="btn btn-success text-white" onClick={() => setModalOpen(true)}>
								Utwórz Ogłoszenie
							</button>
						)}
					</div>
					<div className="overflow-x-auto mt-20">
						<table className="table">
							<thead>
								<tr>
									<th>Nazwa</th>
									<th>Data Utworzenia</th>
									<th>Status</th>
								</tr>
							</thead>
							<tbody>
								{campaign.postings.map((posting) => (
									<tr key={posting.id}>
										<td>
											<Link href={`/campaigns/${campaignId}/postings/${posting.id}`}>{posting.jobName}</Link>
										</td>
										<td>{posting?.datePosted && new Date(posting?.datePosted).toLocaleString()}</td>
										<td>
											<span
												className={`badge badge-xs badge-soft ${postingStatus.get(posting.status) ?? PostingStatus.OPEN}`}
											>
												{postingStatusName.get(posting.status)}
											</span>
										</td>
									</tr>
								))}
							</tbody>
						</table>
					</div>
					<CreatePosting open={modalOpen} campaignId={campaignId} setOpen={setModalOpen} done={fetchCampaign} />
				</>
			)}
		</div>
	);
}
