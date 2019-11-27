<?php
/*
 * Actually, this is not a good place for such code.
 * It would be better to move it into Controller/Presenter.
 * Anyhow, we are doing only a very simple page, so at least
 * we try to separate HTML and PHP as much as possible.
 */

require_once(__DIR__ . '/_data.php');
$data = get_data($order, $desc);
$columns = [
	'name' => 'Name',
	'points' => 'ReCodEx points',
	'memes' => 'Published memes',
];
$orders = [ 0 => '&uarr;', 1 => '&darr;' ];
?>
<h2>Student Results</h2>
<table class="table table-striped mt-5">
	<thead>
		<tr>
			<?php foreach ($columns as $col => $caption) { ?>
			<th class="text-nowrap">
				<span class="mr-2 text-nowrap"><?= $caption ?></span>
				<?php foreach ($orders as $d => $arrow) { ?>
					<a class="btn <?= ($col == $order && $desc == $d) ? 'btn-primary' : 'btn-outline-primary' ?> btn-sm"
						href="?page=results&amp;order=<?= $col ?>&amp;desc=<?= $d ?>"><?= $arrow ?></a>
				<?php } ?>	
			</th>
			<?php } ?>
		</tr>
	</thead>
	<tbody>
		<?php foreach ($data as $item) { ?>
		<tr>
			<td class="w-50"><?= htmlspecialchars("$item->name $item->surname"); ?></td>
			<td class="w-25"><?= $item->points ?></td>
			<td class="w-25"><?= $item->memes ?></td>
		</tr>
		<?php } ?>
	</tbody>
</table>
