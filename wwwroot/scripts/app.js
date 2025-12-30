function resetFieldsetChecked(event) {
    const button = event.target;
    const fieldset = button.closest('fieldset');
    const inputs = fieldset.querySelectorAll('input[type="checkbox"]');
    inputs.forEach(input => input.checked = false);
}

function resetForm(event) {
    const button = event.target;
    const form = button.closest('form');
    const inputs = form.querySelectorAll('input');
    inputs.forEach(input => {
        if (input.type === 'checkbox' || input.type === 'radio') {
            input.checked = false;
        } else {
            input.value = '';
        }
    });
}

function calculateTotal(formName, inputName) {
    const form = document.getElementById(formName);
    if (!form) return 0;
    if (!form.elements[inputName]) return 0;
    if (form.elements[inputName].length === undefined) {
        // Only one row
        return parseFloat(form.elements[inputName].value) || 0;
    } else {
        // Multiple rows
        return Array.from(form.elements[inputName]).reduce((acc, el) => acc + parseFloat(el.value) || 0, 0);
    }

}

function clone(templateId, parentId, element) {
    console.log(element);
    const template = document.getElementById(templateId);
    const parent = document.getElementById(parentId);
    const existingElements = parent.getElementsByClassName(templateId + '-row');
    element.after(template.content.cloneNode(true));
}
