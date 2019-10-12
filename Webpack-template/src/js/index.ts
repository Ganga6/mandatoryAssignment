let getEl = x => document.getElementById(x);

interface Value {
	Id: int;
	Value: string;
}

let arrOfValues: Value[] = [];
let RunFunc = () => {
	let index: number = arrOfValues.length;
	if (!index) {
		index = 1;
	} else {
		index++
	}
	let el = getEl("input");
	let selector = getEl("select");
	let val = el.value;
	val = selector.options.selectedIndex === 0 ? val.toUpperCase() : val.toLowerCase();
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