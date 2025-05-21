// FORM submit LOGIC

const forms = document.querySelectorAll('form');

forms.forEach(form => {


    form.addEventListener('submit', async (e) => {
        e.preventDefault();

        clearErrorMessages(form);

        const formData = new FormData(form);

        try {
            const quillEditor = form.querySelector("#edit-project-description-wysiwyg-editor");
            const quillInstance = window.editProjectDescriptionQuill;

            if (quillEditor && quillInstance) {
                const plainText = quillInstance.getText().trim();
                if (plainText.length === 0) {
                    addErrorMessage(form, "Description", "Required");
                    return; // Stop form submission
                }

                // Update hidden textarea with HTML content from Quill
                const html = quillInstance.root.innerHTML;
                form.querySelector("#edit-project-description").value = html;
            }
            const res = await fetch(form.action, {
                method: 'POST',
                body: formData
            });

            if (res.status === 400) {
                const data = await res.json();

                let errorMap = {};

                // Format A: data.errors as { key: [errors] }
                if (data.errors && !Array.isArray(data.errors)) {
                    errorMap = data.errors;
                }

                // Format B: data.errors as array of { Key, Value }
                if (Array.isArray(data.errors)) {
                    data.errors.forEach(err => {
                        errorMap[err.Key] = err.Value;
                    });
                }

                // Inject error messages into form
                Object.keys(errorMap).forEach(key => {
                    addErrorMessage(form, key, errorMap[key].join('\n'));
                });
            } else if (res.redirected) {
                // Handle a redirect from the server (like RedirectToAction)
                window.location.href = res.url;
            } else {
                // Optional: Reload or redirect manually
                window.location.reload(); // or redirect to a known URL
            }

        } catch (error) {
            console.error("Error submitting the form", error);
        }
    });
});

function clearErrorMessages(form) {
    form.querySelectorAll('[data-val="true"], .form-input, input, textarea, select').forEach(input => {
        input.classList.remove('input-validation-error');
    });

    form.querySelectorAll('span[data-valmsg-for], span.field-validation-error, span.text-danger').forEach(span => {
        span.classList.remove('field-validation-error');
        span.innerText = '';
    });
}

function addErrorMessage(form, key, errorMessage) {
    const input = form.querySelector(`[name="${key}"]`);
    if (input) {
        input.classList.add('input-validation-error');
    }

    const span = form.querySelector(`span[data-valmsg-for="${key}"]`);
    if (span) {
        span.innerText = errorMessage;
        span.classList.add('field-validation-error');
    }
}





// DARK MODE TOGGLE, made with GPT

document.addEventListener('DOMContentLoaded', () => {
    const toggle = document.getElementById('darkModeToggle');
    const darkClass = 'dark';

    if (!toggle) return; 

    if (localStorage.getItem('theme') === 'dark') {
        document.documentElement.classList.add(darkClass);
        toggle.checked = true;
    }

    toggle.addEventListener('change', () => {
        if (toggle.checked) {
            document.documentElement.classList.add(darkClass);
            localStorage.setItem('theme', 'dark');
        } else {
            document.documentElement.classList.remove(darkClass);
            localStorage.setItem('theme', 'light');
        }
    });
});


// Add Project Quill Initialization, made with GPT
document.addEventListener("DOMContentLoaded", function () {
    const addProjectDescriptionTextarea = document.getElementById('add-project-description');
    const addProjectEditor = document.getElementById('add-project-description-wysiwyg-editor');
    const addProjectToolbar = document.getElementById('add-project-description-wysiwyg-toolbar');

    if (addProjectDescriptionTextarea && addProjectEditor && addProjectToolbar) {
        const addProjectDescriptionQuill = new Quill(addProjectEditor, {
            modules: {
                syntax: true,
                toolbar: addProjectToolbar
            },
            theme: 'snow',
            placeholder: 'Type something'
        });

        addProjectDescriptionQuill.on('text-change', function () {
            addProjectDescriptionTextarea.value = addProjectDescriptionQuill.root.innerHTML;
        });
    }

    // Edit Project Quill Initialization
    const editProjectDescriptionTextarea = document.getElementById('edit-project-description');
    const editProjectEditor = document.getElementById('edit-project-description-wysiwyg-editor');
    const editProjectToolbar = document.getElementById('edit-project-description-wysiwyg-toolbar');

    if (editProjectDescriptionTextarea && editProjectEditor && editProjectToolbar) {
        window.editProjectDescriptionQuill = new Quill(editProjectEditor, {
            modules: {
                syntax: true,
                toolbar: editProjectToolbar
            },
            theme: 'snow',
            placeholder: 'Type something'
        });

        window.editProjectDescriptionQuill.on('text-change', function () {
            editProjectDescriptionTextarea.value = window.editProjectDescriptionQuill.root.innerHTML;
        });
    }
});


// IMAGE UPLOAD LOGIC

const uploadTrigger = document.getElementById('upload-trigger');
const fileInput = document.getElementById('image-upload');
const imagePreview = document.getElementById('image-preview');
const imagePreviewIconCotnainer = document.getElementById('image-preview-icon-container');
const imagePreviewIcon = document.getElementById('image-preview-icon');

if (uploadTrigger && fileInput) {
    uploadTrigger.addEventListener('click', function () {
        fileInput.click();
    });

    fileInput.addEventListener('change', function (e) {
        const file = e.target.files[0];
        if (file && file.type.startsWith('image/')) {
            const reader = new FileReader();

            reader.onload = (e) => {
                imagePreview.src = e.target.result;
                imagePreview.classList.remove('hide');
                imagePreviewIconCotnainer.classList.add('selected');
                imagePreviewIcon.classList.remove('fa-camera');
                imagePreviewIcon.classList.add('fa-edit');
            };

            reader.readAsDataURL(file);
        }
    });
}



const editUploadTrigger = document.getElementById('edit-upload-trigger');
const editFileInput = document.getElementById('edit-image-upload');
const editImagePreview = document.getElementById('edit-image-preview');
const editImagePreviewIconCotnainer = document.getElementById('edit-image-preview-icon-container');
const editImagePreviewIcon = document.getElementById('edit-image-preview-icon');

if (editUploadTrigger && editFileInput) {
    editUploadTrigger.addEventListener('click', function () {
        editFileInput.click();
    });

    editFileInput.addEventListener('change', function (e) {
        const file = e.target.files[0];
        if (file && file.type.startsWith('image/')) {
            const reader = new FileReader();

            reader.onload = (e) => {
                editImagePreview.src = e.target.result;
                editImagePreview.classList.remove('hide');
                editImagePreviewIconCotnainer.classList.add('selected');
                editImagePreviewIcon.classList.remove('fa-camera');
                editImagePreviewIcon.classList.add('fa-edit');
            };

            reader.readAsDataURL(file);
        }
    });
}



// HEADER DROPDOWN MENU LOGIC

const dropdowns = document.querySelectorAll('[data-type="dropdown"]')

document.addEventListener('click', function (event) {
    let clickedDropdown = null

    dropdowns.forEach(dropdown => {
        const targetId = dropdown.getAttribute('data-target')
        const targetElement = document.querySelector(targetId)

        if (dropdown.contains(event.target)) {
            clickedDropdown = targetElement

            document.querySelectorAll('.dropdown.dropdown-show').forEach(openDropdown => {
                if (openDropdown !== targetElement) {
                    openDropdown.classList.remove('dropdown-show')
                }
            })
            targetElement.classList.toggle('dropdown-show')
        }
    })

    if (!clickedDropdown && !event.target.closest('.dropdown')) {
        document.querySelectorAll('.dropdown.dropdown-show').forEach(openDropdown => {
            openDropdown.classList.remove('dropdown-show')
        })
    }
})

// ADD PROJECT MODAL LOGIC

const modals = document.querySelectorAll('[data-type="modal"]')
modals.forEach(modal => {
    modal.addEventListener('click', function () {
        const targetId = modal.getAttribute('data-target')
        const targetElement = document.querySelector(targetId)

        targetElement.classList.add('modal-show')
    })
})

// CLOSE BUTTON LOGIC

const closeButtons = document.querySelectorAll('[data-type="close"]')
closeButtons.forEach(button => {
    button.addEventListener('click', function () {
        const targetId = button.getAttribute('data-target')
        const targetElement = document.querySelector(targetId)

        targetElement.classList.remove('modal-show')
    })
})


// SELECTOR LOGIC (i.e. client selection)

document.querySelectorAll('.form-select').forEach(select => {
    const trigger = select.querySelector('.form-select-trigger')
    const triggerText = trigger.querySelector('.form-select-text')
    const options = select.querySelectorAll('.form-select-option')
    const hiddenInput = select.querySelector('input[type=hidden]')
    const placeholder = select.dataset.placeholder || "Choose"

    const setValue = (value = "", text = placeholder) => {
        triggerText.textContent = text
        hiddenInput.value = value
        select.classList.toggle('has-placeholder', !value)
    };

    setValue();

    trigger.addEventListener('click', (e) => {
        e.stopPropagation();
        document.querySelectorAll('.form-select.open').forEach(el => {
            if (el !== select) el.classList.remove('open');
        });
        select.classList.toggle('open');
    })

    options.forEach(option =>
        option.addEventListener('click', () => {
            setValue(option.dataset.value, option.textContent)
            select.classList.remove('open')
        })
    )

    document.addEventListener('click', e => {
        if (!select.contains(e.target))
            select.classList.remove('open')
    })
})