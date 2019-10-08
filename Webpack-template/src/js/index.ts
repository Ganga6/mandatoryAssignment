let getEl = x => document.getElementById(x);

interface Value {
	Id: int;
	Value: string;
}

let arrOfValues: Value[] = [];

let RunFunc = () => {
	let index: int = arrOfValues.length;
	if (!index) {
		index = 1;
	} else {
		index++
	}
	let el = getEl("input");
	let selector = getEl("select");
	let val = el.value;
	switch(selector.options.selectedIndex){
		case 0:
			val = val.toUpperCase();
			break;
		case 1:
			val = val.toLowerCase();
			break;
	}
	arrOfValues.push({Id: index, Value: val });
	let divWrapper = document.createElement("div");
	divWrapper.classList.add('value');

	let indexEl = document.createElement("span");
	indexEl.innerHTML = index;

	let valEl = document.createElement("span");
	valEl.innerHTML = val;

	divWrapper.appendChild(indexEl);
	divWrapper.appendChild(valEl);
	getEl("result").appendChild(divWrapper);
}

getEl("btn").addEventListener("click", RunFunc);