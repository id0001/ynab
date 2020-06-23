'use strict';

const PRIMES = {
	32   : BigInt(16777619),
	64   : BigInt(1099511628211),
	128  : BigInt(309485009821345068724781371),
	256  : BigInt(374144419156711147060143317175368453031918731002211),
	512  : BigInt(
		35835915874844867368919076489095108449946327955754392558399825615420669938882575126094039892345713852759
	),
	1024 : BigInt(
		5016456510113118655434598811035278955030765345404790744303017523831112055108147451509157692220295382716162651878526895249385292291816524375083746691371804094271873160484737966720260389217684476157468082573
	)
};

const OFFSETS = {
	32   : BigInt(2166136261),
	64   : BigInt(14695981039346656037),
	128  : BigInt(144066263297769815596495629667062367629),
	256  : BigInt(100029257958052580907070968620625704837092796014241193945225284501741471925557),
	512  : BigInt(
		9659303129496669498009435400716310466090418745672637896108374329434462657994582932197716438449813051892206539805784495328239340083876191928701583869517785
	),
	1024 : BigInt(
		14197795064947621068722070641403218320880622795441933960878474914617582723252296732303717722150864096521202355549365628174669108571814760471015076148029755969804077320157692458563003215304957150157403644460363550505412711285966361610267868082893823963790439336411086884584107735010676915
	)
};

export const fnv1a = (string, { size = 32 } = {}) => {
	if (!PRIMES[size]) {
		throw new Error('The `size` option must be one of 32, 64, 128, 256, 512, or 1024');
	}

	let hash = OFFSETS[size];
	const fnvPrime = PRIMES[size];

	for (let i = 0; i < string.length; i++) {
		let cc = string.charCodeAt(i);
		hash ^= BigInt(cc);
		hash = BigInt.asUintN(size, hash * fnvPrime);
	}

	return hash;
};

export default fnv1a;
