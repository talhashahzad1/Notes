///<reference path="../../node_modules/@types/jquery/index.d.ts" />
declare var ace: any;

$("[data-delete]").on("click", (event) => {
    event.preventDefault();
    let $target = $(event.target);
    if ($target.data("confirm") && !confirm($target.data("confirm"))) {
        return;
    }
    $.ajax({
        url: $target.attr("href"),
        method: "DELETE",
        success: () => {
            window.location.href = $target.data("return-url");
        }
    });
});

let textarea = $("#NoteForm #Body");
if (textarea.length) {
    textarea.hide();
    let editor = ace.edit("NoteEditor");
    editor.setTheme("ace/theme/github");
    editor.getSession().setMode("ace/mode/markdown");
    editor.getSession().setValue(textarea.val());
    editor.getSession().on("change", () => {
        textarea.val(editor.getSession().getValue());
    });
}