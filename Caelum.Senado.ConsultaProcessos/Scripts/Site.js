//disparada quando modalAcompanhar estiver prestes a ser mostrada
$('#modalAcompanhar').on('show.bs.modal', function (e) {

    //get data-id attribute of the clicked element
    var processoId = $(e.relatedTarget).data('processo-id');
    var processoCodigo = $(e.relatedTarget).data('processo-codigo');

    console.log("ProcessoId: " + processoId);
    console.log("ProcessoCodigo: " + processoCodigo);

    //populate the textbox
    $(e.currentTarget).find('input[name="processoId"]').val(processoId);
    $(e.currentTarget).find('#modal-processo-codigo').text(processoCodigo);

    $(e.currentTarget).find('input[name="nome"]').val("");
    $(e.currentTarget).find('input[name="email"]').val("");
});

//disparada quando modalExcluir estiver prestes a ser mostrada
$('#modalExcluir').on('show.bs.modal', function (e) {

    //get data-id attribute of the clicked element
    var processoId = $(e.relatedTarget).data('processo-id');
    var processoCodigo = $(e.relatedTarget).data('processo-codigo');

    console.log("ProcessoId: " + processoId);
    console.log("ProcessoCodigo: " + processoCodigo);

    //populate the textbox
    $(e.currentTarget).find('input[name="id"]').val(processoId);
    $(e.currentTarget).find('#excluir-processo-codigo').text(processoCodigo);
});