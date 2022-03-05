import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:font_awesome_flutter/font_awesome_flutter.dart';
import 'package:inventory/models/inventory.dart';
import 'package:inventory/provider/online_data_provider.dart';

final inventoryStreamProvider = StreamProvider<List<Inventory>>((ref) {
  final inventoryData = ref.watch(inventoryProvider);
  return inventoryData.allItems;
});

class InventoryPage extends ConsumerWidget {
  const InventoryPage({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final stream = ref.watch(inventoryStreamProvider);
    return Scaffold(
      body: stream.when(
        data: _buildItemsList,
        error: (e, stack) => Text('Error: $e'),
        loading: () => const CircularProgressIndicator(),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () {},
        child: const FaIcon(FontAwesomeIcons.bars),
        mini: true,
      ),
    );
  }

  Widget _buildItemsList(List<Inventory> items) {
    return ListView.separated(
      itemBuilder: (context, index) {
        final currentItem = items[index];
        return Padding(
          padding: const EdgeInsets.symmetric(horizontal: 20),
          child: ListTile(
            title: Text(currentItem.name!),
            subtitle: Text(currentItem.description ?? ''),
            leading: CircleAvatar(
              child: Text(currentItem.quantity!.toString()),
              backgroundColor: Theme.of(context).primaryColorDark,
              foregroundColor: Theme.of(context).primaryColorLight,
            ),
            trailing: Text(currentItem.stockLocation ?? 'N/A'),
          ),
        );
      },
      separatorBuilder: (context, index) => const Divider(),
      itemCount: items.length,
    );
  }
}
